using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

using Bitnet.Client;

using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Controls;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/*
 * Developed using the following resources
 * Bitnet - https://sourceforge.net/projects/bitnet
 * QrCode.Net - http://qrcodenet.codeplex.com/
 * Hotkey = http://www.dreamincode.net/forums/topic/180436-global-hotkeys/
 * 
 */
namespace Bitcoin_QR_Popup
{
    public partial class Form1 : Form
    {

        [Serializable]
        private class exchange_rate
        {
            public double rate = 0;
            public DateTime time_fetched = new DateTime(1970, 1, 1);
        }

        private class calculated_rate
        {
            public double rate = 0;
            public string from = "";
            public string to = "";
        }

        private static string common_openexchangerates_codes_presplit = "AUD,BRL,CAD,CHF,CNY,CZK,DKK,EUR,GBP,HKD,HUF,ILS,JPY,MYR,MXN,NOK,NZD,PHP,PLN,RUB,SGD,SEK,THB,TRY,TWD,USD";
        private static string less_common_openexchangerates_codes_presplit = "AED,AFN,ALL,AMD,ANG,AOA,ARS,AWG,AZN,BAM,BBD,BDT,BGN,BHD,BIF,BMD,BND,BOB,BSD,BTN,BWP,BYR,BZD,CDF,CLF,CLP,CNH,COP,CRC,CUP,CVE,DJF,DOP,DZD,EGP,ETB,FJD,FKP,GEL,GHS,GIP,GMD,GNF,GTQ,GYD,HNL,HRK,HTG,IDR,IEP,INR,IQD,IRR,ISK,JMD,JOD,KES,KGS,KHR,KMF,KPW,KRW,KWD,KZT,LAK,LBP,LKR,LRD,LSL,LTL,LVL,LYD,MAD,MDL,MGA,MKD,MMK,MNT,MOP,MRO,MUR,MVR,MWK,MZN,NAD,NGN,NIO,NPR,OMR,PAB,PEN,PGK,PKR,PYG,QAR,RON,RSD,RWF,SAR,SBD,SCR,SDG,SHP,SLL,SOS,SRD,STD,SVC,SYP,SZL,TJS,TMT,TND,TOP,TTD,TZS,UAH,UGX,UYU,UZS,VEF,VND,VUV,WST,XAF,XAG,XAU,XCD,XCP,XDR,XOF,XPD,XPF,XPT,YER,ZAR,ZMK,ZWL";
        private static string[] common_openexchangerate_codes = common_openexchangerates_codes_presplit.Split(',');
        private static string[] less_common_openexchangerate_codes = less_common_openexchangerates_codes_presplit.Split(',');
        private BitnetClient bc = new BitnetClient("http://127.0.0.1:8332");
        private Dictionary<string, exchange_rate> exchange_rates = new Dictionary<string,exchange_rate>();
        private string bitcoin_account = "";
        private bool bitcoin_is_running = false;

        public Form1()
        {
            InitializeComponent();
            init_exchange_rates();
            bitcoin_account = GetMacAddress();
            set_default_currency();
            initialise_notify_icon_right_click();
            reset_qr_display();
            initialise_hotkey();
            check_bitcoin_is_running();
            set_credentials();
            check_wallet_has_passphrase();
            hide_popup();
        }

        private void init_exchange_rates()
        {
            try
            {
                load_exchange_rates_from_settings();
            }
            catch
            {
                fetch_currency_exchange_rates();
                fetch_bitcoin_rate();
            }
        }

        
        private void perform_exit()
        {
            if (can_exit)
            {
                this.Close();
            }
        }

        private void set_default_currency()
        {
            label_currency.Text = Properties.Settings.Default.default_currency;
        }

        private void initialise_notify_icon_right_click()
        {
            foreach (string currency_code in common_openexchangerate_codes)
            {
                create_currency_notify_item(currency_code);
            }
            toolStripMenuItem_currency.DropDownItems.Add(new ToolStripSeparator());
            foreach (string currency_code in less_common_openexchangerate_codes)
            {
                create_currency_notify_item(currency_code);
            }
        }

        private void create_currency_notify_item(string currency_code)
        {
            ToolStripMenuItem t = new ToolStripMenuItem(currency_code);
            t.Click += new EventHandler(handle_currency_select);
            toolStripMenuItem_currency.DropDownItems.Add(t);
        }

        private void handle_currency_select(object sender, EventArgs e)
        {
            ToolStripMenuItem t = (ToolStripMenuItem)sender;
            Properties.Settings.Default.default_currency = t.Text;
            Properties.Settings.Default.Save();
            label_currency.Text = t.Text;
            update_popup();
            update_exchange_rate();
        }

        private void reset_qr_display()
        {
            Bitmap b = new Bitmap(pictureBox_qr.Width, pictureBox_qr.Height);
            pictureBox_qr.Image = b;
        }

                private void set_credentials()
        {
            //TODO this is only valid for local. When remote is introduced,
            // this info will have to be entered manually.
            if (bitcoin_is_running)
            {
                string user_app_data_path= Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string[] bitcoin_path = new string[] { user_app_data_path, "Bitcoin", "bitcoin.conf" };
                string path = Path.Combine(bitcoin_path);
                if (!File.Exists(path))
                {
                    TopMostMessageBox.Show("Unable to read config. Ensure the file exists, then restart bitcoin.\n\n" + path, "Bitcoin QR Popup", MessageBoxButtons.OK); //TODO translate
                    can_exit = true;
                    return;
                }
                string[] lines = File.ReadAllLines(path);
                string rpcuser = "";
                string rpcpassword = "";
                foreach (string line in lines)
                {
                    string tidy_line = line.Trim();
                    if (tidy_line.StartsWith("rpcuser") && line.Contains("="))
                    {
                        rpcuser = tidy_line.Split('=')[1];
                    }
                    else if (tidy_line.StartsWith("rpcpassword") && line.Contains("="))
                    {
                        rpcpassword = tidy_line.Split('=')[1];
                    }
                    if (rpcuser.Length > 0 && rpcpassword.Length > 0)
                    {
                        break;
                    }
                }
                if (rpcuser == "" || rpcpassword == "")
                {
                    TopMostMessageBox.Show("Unable to read rpc user and password. Ensure the file contains the following text (substituting your own values for 'bob' and 'secret').\n\nrpcuser=bob\nrpcpassword=secret", "Bitcoin QR Popup", MessageBoxButtons.OK); //TODO translate
                    can_exit = true;
                    return;
                }
                //TODO check that this wallet has a password on it
                bc.Credentials = new NetworkCredential(rpcuser, rpcpassword);
            }
        }

        private void check_wallet_has_passphrase()
        {
            if (!can_exit)
            {
                JObject j = bc.WalletPassphrase("a_string_which_is_definitely_not_the_password", 1);
                if (j.HasValues)
                {
                    string error = j["error"]["message"].ToString();
                    if (error.Contains("incorrect"))
                    {
                        //Nothing to do - they have made a passphrase
                    }
                    else
                    {
                        TopMostMessageBox.Show("You must set a passphrase on your wallet.\n\nOpen the client and select Settings > Encrypt Wallet.", "Bticoin QR Popup", MessageBoxButtons.OK); //TODO translate
                        can_exit = true;
                    }
                }
            }
        }

        // see http://www.daniweb.com/software-development/csharp/threads/221450/how-to-get-the-path-of-a-running-process
        private void check_bitcoin_is_running()
        {
            Process[] processes = Process.GetProcesses();
            string executable_path = "";
            bool bitcoinqt_is_running = false;
            foreach (Process p in processes)
            {
                string procname = "";
                try
                {
                    procname = p.ProcessName;
                }
                catch (Win32Exception)
                {
                    procname = "n/a";
                }
                if (procname == "bitcoind") //TODO how to tell if running "bitcoin -server"
                {
                    bitcoin_is_running = true;
                    executable_path = Path.GetDirectoryName(p.MainModule.FileName);
                    break;
                }
                else if (procname == "bitcoin-qt")
                {
                    bitcoinqt_is_running = true;
                }
            }
            string daemon_path = "C:\\Program Files (x86)\\Bitcoin\\daemon\\bitcoind.exe";
            if (bitcoinqt_is_running)
            {
                TopMostMessageBox.Show("You need to run the daemon version of bitcoin. It's at \n\n" + daemon_path, "Bitcoin QR Popup", MessageBoxButtons.OK); //TODO translate
                can_exit = true;
            }
            else if(!bitcoin_is_running)
            {
                TopMostMessageBox.Show("Unable to connect to bitcoin. Run it from\n\n" + daemon_path, "Bitcoin QR Popup", MessageBoxButtons.OK); //TODO translate
                can_exit = true;
            }
        }

        private void show_popup()
        {
            try
            {
                WindowState = FormWindowState.Normal;
                textBox_amount.Focus();
                textBox_amount.Text = "";
                label_amount.Text = "0";
                textBox_address.Text = "Fetching address"; //TODO translate
                reset_qr_display();
                update_exchange_rate();
                Cursor.Current = Cursors.AppStarting;
                Application.DoEvents();
                string address = bc.GetNewAddress(bitcoin_account); // "1FiVXpdBdcnv6GAVZDKRxeSJqZQrSx2s2Q"; // 
                Cursor.Current = Cursors.Default;
                textBox_address.Text = address;
                draw_qr(address);
                timer_poll_amount.Start();
            }
            catch
            {
                //TODO display an error on the image
                //and make this generally more useful
                textBox_address.Text = "Unknown error";
                draw_qr("Unknown error");
            }
        }

        private void hide_popup()
        {
            timer_poll_amount.Stop();
            WindowState = FormWindowState.Minimized;
            textBox_amount.Text = "";
        }

        private void draw_qr(string qr_value)
        {
            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
            QrCode qrCode = qrEncoder.Encode(qr_value);

            int padding = 2;
            double module_size = (double)(pictureBox_qr.Width) / (double)(qrCode.Matrix.Width + 2*padding);

            int moduleSizeInPixels = (int)Math.Floor(module_size);
            Renderer renderer = new Renderer(moduleSizeInPixels, Brushes.Black, Brushes.White);
            renderer.QuietZoneModules = (QuietZoneModules)padding;
            
            Size qrCodeSize = renderer.Measure(qrCode.Matrix.Width);
            reset_qr_display();
            using (Graphics graphics = Graphics.FromImage(pictureBox_qr.Image))
            {
                renderer.Draw(graphics, qrCode.Matrix);
            }

            pictureBox_qr.Refresh();
        }

        private void textBox_amount_TextChanged(object sender, EventArgs e)
        {
            update_popup();
        }

        private void update_popup()
        {
            try
            {
                string address = textBox_address.Text;
                string amount = get_amount();
                string uri = get_bitcoin_uri(address, amount);
                label_amount.Text = amount; //TODO this is the wrong symbol, but close enough for now
                draw_qr(uri);
            }
            catch
            {
                //TODO display a meaningful error
                label_amount.Text = "0";
                draw_qr(textBox_address.Text);
            }
        }

        private string get_bitcoin_uri(string address, string amount)
        {
            string uri = "bitcoin:" + address + "?amount=" + amount;
            return uri;
        }

        private string get_amount()
        {
            string s_local_amount = textBox_amount.Text;
            double local_amount = Convert.ToDouble(s_local_amount);
            double exchange_rate = update_exchange_rate();
            double bitcoin_amount = local_amount * exchange_rate;
            return bitcoin_amount.ToString("0.00000000");
        }

        private double update_exchange_rate()
        {
            calculated_rate c = calculate_exchange_rate(label_currency.Text, "BTC");
            label_exchangeA.Text = c.from + " / " + c.to + " = " + (1.0 / c.rate).ToString("0.000");
            label_exchangeB.Text = c.to + " / " + c.from + " = " + c.rate.ToString("0.000");
            return c.rate;
        }

        private calculated_rate calculate_exchange_rate(string from_code, string to_code)
        {
            double from_FROM_to_USD = 1.0 / exchange_rates[from_code].rate;
            double from_USD_to_TO = exchange_rates[to_code].rate;
            double calculated_rate = from_FROM_to_USD * from_USD_to_TO;
            calculated_rate rate = new calculated_rate();
            rate.from = from_code;
            rate.to = to_code;
            rate.rate = calculated_rate;
            return rate;
        }


        private void fetch_currency_exchange_rates()
        {
            bool need_to_fetch = check_currencies_must_be_updated();
            if (need_to_fetch)
            {
                string url = "http://openexchangerates.org/latest.json";
                string data = urlread(url);
                DateTime time_fetched = DateTime.Now;
                JObject j = JsonConvert.DeserializeObject<JObject>(data);
                foreach (JProperty rate in j["rates"])
                {
                    string code = rate.Name;
                    double rate_value = Convert.ToDouble(rate.Value.ToString());
                    if (!(exchange_rates.Keys.Contains(code)))
                    {
                        exchange_rates[code] = new exchange_rate();
                    }
                    exchange_rates[code].rate = rate_value;
                    exchange_rates[code].time_fetched = time_fetched;
                }
                save_exchange_rates_in_settings();
            }
        }

        private bool check_currencies_must_be_updated()
        {
            bool need_to_fetch = check_currency_list_for_refresh(common_openexchangerate_codes);
            need_to_fetch = need_to_fetch || check_currency_list_for_refresh(less_common_openexchangerate_codes);
            return need_to_fetch;
        }

        private bool check_currency_list_for_refresh(string[] currency_list)
        {
            int refetch_interval_minutes = 60;
            bool need_to_fetch = false;
            foreach (string code in currency_list)
            {
                if (!(exchange_rates.Keys.Contains(code)))
                {
                    exchange_rates[code] = new exchange_rate();
                    need_to_fetch = true;
                }
                else if (exchange_rates[code].time_fetched.AddMinutes(refetch_interval_minutes) < DateTime.Now)
                {
                    need_to_fetch = true;
                }
            }
            return need_to_fetch;
        }

        private void fetch_bitcoin_rate()
        {
            int refetch_interval_minutes = 60;
            bool need_to_fetch = false;
            if (!exchange_rates.Keys.Contains("BTC"))
            {
                exchange_rates["BTC"] = new exchange_rate();
                need_to_fetch = true;
            }
            else if (exchange_rates["BTC"].time_fetched.AddMinutes(refetch_interval_minutes) < DateTime.Now)
            {
                need_to_fetch = true;
            }
            if (need_to_fetch)
            {
                string url = "http://www.bitcoincharts.com/t/weighted_prices.json";
                string data = urlread(url);
                JObject j = JsonConvert.DeserializeObject<JObject>(data);
                exchange_rates["BTC"].rate = 1.0 / Convert.ToDouble(j["USD"]["24h"].ToString());
                exchange_rates["BTC"].time_fetched = DateTime.Now;
                save_exchange_rates_in_settings();
            }
        }

        private void save_exchange_rates_in_settings()
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, exchange_rates);
            string str = System.Convert.ToBase64String(memoryStream.ToArray());
            Properties.Settings.Default.exchange_rates = str;
            Properties.Settings.Default.Save();
        }

        private void load_exchange_rates_from_settings()
        {
            string s_exchange_rates = Properties.Settings.Default.exchange_rates;

            byte[] b = System.Convert.FromBase64String(s_exchange_rates);
            MemoryStream memoryStream = new MemoryStream(b);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            object o = binaryFormatter.Deserialize(memoryStream);
            exchange_rates = (Dictionary<string, exchange_rate>)o;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!can_exit)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            hide_popup();
        }

        private bool can_exit = false;
        private void toolStripMenuItem_exit_Click(object sender, EventArgs e)
        {
            can_exit = true;
            perform_exit();
            return;
        }

        private void notifyIcon_bitcoin_qr_popup_Click(object sender, EventArgs e)
        {
            MouseEventArgs m = (MouseEventArgs)e;
            if (m.Button == System.Windows.Forms.MouseButtons.Left)
            {
                toggle_display();
            }
        }

        private void toggle_display()
        {
            if (WindowState == FormWindowState.Normal)
            {
                hide_popup();
            }
            else
            {
                show_popup();
            }
        }

        private void display_history()
        {
            JArray transactions = bc.ListTransactions(bitcoin_account, 5);
            string message = "";
            if (transactions.Count > 0)
            {
                foreach (JObject t in transactions)
                {
                    double btc_amount = Convert.ToDouble(t["amount"].ToString());
                    double exchange_rate = calculate_exchange_rate("BTC", label_currency.Text).rate;
                    double local_value = btc_amount * exchange_rate;
                    DateTime transaction_time = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(Convert.ToDouble(t["time"].ToString())).ToLocalTime();
                    DateTime now = DateTime.Now;
                    bool is_from_today = now.Date == transaction_time.Date;
                    if (is_from_today)
                    {
                        string transaction = transaction_time.ToString("HH:mm:ss") + " - " + btc_amount.ToString("0.00") + " BTC / " + local_value.ToString("0.00") + " " + label_currency.Text + "\n";
                        message = transaction + message;
                    }
                }
                if (message.Length == 0)
                {
                    message = "No transactions to show\n"; //TODO translate
                }
                
            }
            else
            {
                 message = "No transactions to show\n"; //TODO translate
            }
            message = message + "\n" + "Time now: " + DateTime.Now.ToString("HH:mm:ss");
            TopMostMessageBox.Show(message, "Transaction History"); //TODO translate
        }
























        //From http://support.microsoft.com/kb/307023

        private string urlread(string sURL)
        {
            string web_data = "";
            
            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(sURL);

            WebProxy myProxy = new WebProxy("myproxy", 80);
            myProxy.BypassProxyOnLocal = true;

            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();

            StreamReader objReader = new StreamReader(objStream);

            string sLine = "";
            int i = 0;

            while (sLine != null)
            {
                i++;
                sLine = objReader.ReadLine();
                if (sLine != null)
                    web_data = web_data + sLine + Environment.NewLine;
            }

            return web_data;
        }

        private void timer_poll_amount_Tick(object sender, EventArgs e)
        {
            string address = textBox_address.Text;
            int dp = 5; //Some clients only send to 5dp... bitcoinspinner
            double amount = Math.Round(Convert.ToDouble(label_amount.Text), dp);
            double received = Math.Round((double)bc.GetReceivedByAddress(address, 0), dp);
            if (amount > 0 && received >= amount)
            {
                timer_poll_amount.Stop();
                TopMostMessageBox.Show("Payment received - ฿ " + received.ToString(), "Payment received");
                hide_popup();
            }
        }


















        /* Manipulated from http://www.dreamincode.net/forums/topic/180436-global-hotkeys/ */

        public const int WM_HOTKEY_MSG_ID = 0x0312;
        private GlobalHotkey hide_show_hotkey;
        private GlobalHotkey history_hotkey;

        private const Keys HIDE_SHOW_HOTKEY = Keys.F2;
        private const Keys HISTORY_HOTKEY = Keys.F3;

        private void initialise_hotkey()
        {
            hide_show_hotkey = new GlobalHotkey(0, HIDE_SHOW_HOTKEY, this);
            history_hotkey = new GlobalHotkey(0, HISTORY_HOTKEY, this);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY_MSG_ID)
            {
                switch ((Keys)((m.LParam.ToInt32()) >> 16))
                {
                    case HISTORY_HOTKEY:
                        display_history();
                        break;
                    case HIDE_SHOW_HOTKEY:
                        toggle_display();
                        break;
                }
            }
            base.WndProc(ref m);
        }

        public class GlobalHotkey
        {
            private int modifier;
            private int key;
            private IntPtr hWnd;
            private int id;

            public GlobalHotkey(int modifier, Keys key, Form form)
            {
                this.modifier = modifier;
                this.key = (int)key;
                this.hWnd = form.Handle;
                id = this.GetHashCode();
            }

            public bool Register()
            {
                return RegisterHotKey(hWnd, id, modifier, key);
            }

            public bool Unregister()
            {
                return UnregisterHotKey(hWnd, id);
            }

            public override int GetHashCode()
            {
                return modifier ^ key ^ hWnd.ToInt32();
            }

            [DllImport("user32.dll")]
            private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

            [DllImport("user32.dll")]
            private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            hide_show_hotkey.Register();
            history_hotkey.Register();
            if (can_exit)
            {
                this.Close();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            hide_show_hotkey.Unregister();
            history_hotkey.Unregister();
        }

        

















        /* from http://stackoverflow.com/questions/850650/reliable-method-to-get-machines-mac-address-in-c-sharp */

        private string GetMacAddress()
        {
            const int MIN_MAC_ADDR_LENGTH = 12;
            string macAddress = "";
            long maxSpeed = -1;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                string tempMac = nic.GetPhysicalAddress().ToString();
                if (nic.Speed > maxSpeed && !String.IsNullOrEmpty(tempMac) && tempMac.Length >= MIN_MAC_ADDR_LENGTH)
                {
                    maxSpeed = nic.Speed;
                    macAddress = tempMac;
                }
            }
            return macAddress;
        }



















        static public class TopMostMessageBox
        {
            static public DialogResult Show(string message)
            {
                return Show(message, string.Empty, MessageBoxButtons.OK);
            }

            static public DialogResult Show(string message, string title)
            {
                return Show(message, title, MessageBoxButtons.OK);
            }

            static public DialogResult Show(string message, string title,
                MessageBoxButtons buttons)
            {
                // Create a host form that is a TopMost window which will be the 
                // parent of the MessageBox.
                Form topmostForm = new Form();
                // We do not want anyone to see this window so position it off the 
                // visible screen and make it as small as possible
                topmostForm.Size = new System.Drawing.Size(1, 1);
                topmostForm.StartPosition = FormStartPosition.Manual;
                System.Drawing.Rectangle rect = SystemInformation.VirtualScreen;
                topmostForm.Location = new System.Drawing.Point(rect.Bottom + 10,
                    rect.Right + 10);
                topmostForm.Show();
                // Make this form the active form and make it TopMost
                topmostForm.Focus();
                topmostForm.BringToFront();
                topmostForm.TopMost = true;
                // Finally show the MessageBox with the form just created as its owner
                DialogResult result = MessageBox.Show(topmostForm, message, title,
                    buttons);
                topmostForm.Dispose(); // clean it up all the way

                return result;
            }
        }
     
    }
}
