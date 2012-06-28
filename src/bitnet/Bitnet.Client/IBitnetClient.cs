// COPYRIGHT 2011 Konstantin Ineshin, Irkutsk, Russia.
// If you like this project please donate BTC 18TdCC4TwGN7PHyuRAm8XV88gcCmAHqGNs

using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;


namespace Bitnet.Client
{
  interface IBitnetClient
  {
    /// <summary>
    /// Safely copies wallet.dat to destination, which can be a directory or a path with filename.
    /// </summary>
    /// <param name="a_destination"></param>
    void BackupWallet(string a_destination);

    /// <summary>
    /// Returns the account associated with the given address.
    /// </summary>
    /// <param name="a_address"></param>
    string GetAccount(string a_address);

    /// <summary>
    /// Returns the current bitcoin address for receiving payments to this account.
    /// </summary>
    /// <param name="a_account"></param>
    string GetAccountAddress(string a_account);

    /// <summary>
    /// Returns the list of addresses for the given account.
    /// </summary>
    /// <param name="a_account"></param>
    IEnumerable<string> GetAddressesByAccount(string a_account);

    /// <summary>
    /// If [account] is not specified, returns the server's total available balance.
    /// If [account] is specified, returns the balance in the account.
    /// </summary>
    /// <param name="a_account"></param>
    /// <param name="a_minconf"></param>
    float GetBalance(string a_account = "", int a_minconf = 1);

    /// <summary>
    /// Dumps the block existing at specified height. 
    /// </summary>
    /// <param name="a_height"></param>
    string GetBlockByCount(int a_height);

    /// <summary>
    /// Returns the number of blocks in the longest block chain.
    /// </summary>
    int GetBlockCount();

    /// <summary>
    /// Returns the block number of the latest block in the longest block chain.
    /// </summary>
    int GetBlockNumber();

    /// <summary>
    /// Returns the number of connections to other nodes.
    /// </summary>
    int GetConnectionCount();

    /// <summary>
    /// Returns the proof-of-work difficulty as a multiple of the minimum difficulty.
    /// </summary>
    float GetDifficulty();

    /// <summary>
    /// Returns true or false whether bitcoind is currently generating hashes
    /// </summary>
    bool GetGenerate();

    /// <summary>
    /// Returns a recent hashes per second performance measurement while generating.
    /// </summary>
    float GetHashesPerSec();

    /// <summary>
    /// Returns an object containing various state info.
    /// </summary>
    JObject GetInfo();

    /// <summary>
    /// Returns a new bitcoin address for receiving payments.
    /// If [account] is specified (recommended), it is added to the address book so payments 
    /// received with the address will be credited to [account].
    /// </summary>
    /// <param name="a_account"></param>
    string GetNewAddress(string a_account);

    /// <summary>
    /// Returns the total amount received by addresses with accountin 
    /// transactions with at least [minconf] confirmations.
    /// </summary>
    /// <param name="a_account"></param>
    /// <param name="a_minconf"></param>
    float GetReceivedByAccount(string a_account, int a_minconf = 1);

    /// <summary>
    /// Returns the total amount received by bitcoinaddressin transactions 
    /// with at least [minconf] confirmations.
    /// </summary>
    /// <param name="a_address"></param>
    /// <param name="a_minconf"></param>
    float GetReceivedByAddress(string a_address, int a_minconf = 1);

    /// <summary>
    /// Get detailed information about txid
    /// </summary>
    /// <param name="a_txid"></param>
    JObject GetTransaction(string a_txid);
    
    /// <summary>
    /// Tries to solve the block and returns true if it was successful.
    /// </summary>
    JObject GetWork();

    /// <summary>
    /// Returns formatted hash data to work on
    /// </summary>
    /// <param name="a_data"></param>
    bool GetWork(string a_data);

    /// <summary>
    /// List commands, or get help for a command.
    /// </summary>
    /// <param name="a_command"></param>
    string Help(string a_command = "");

    /// <summary>
    /// Returns Object that has account names as keys, account balances as values.
    /// </summary>
    /// <param name="a_minconf"></param>
    JObject ListAccounts(int a_minconf = 1);

    /// <summary>
    /// Returns an array of objects containing:
    /// </summary>
    /// <param name="a_minconf"></param>
    /// <param name="a_includeEmpty"></param>
    JArray ListReceivedByAccount(int a_minconf = 1, bool a_includeEmpty = false);

    /// <summary>
    /// Returns an array of objects containing:
    /// </summary>
    /// <param name="minconf"></param>
    /// <param name="a_includeEmpty"></param>
    JArray ListReceivedByAddress(int a_minconf = 1, bool a_includeEmpty = false);

    /// <summary>
    /// Returns up to [count] most recent transactions for account account.
    /// </summary>
    /// <param name="account"></param>
    /// <param name="a_count"></param>
    JArray ListTransactions(string a_account, int a_count = 10);

    /// <summary>
    /// Move from one account in your wallet to another.
    /// </summary>
    /// <param name="a_fromAccount"></param>
    /// <param name="a_toAccount"></param>
    /// <param name="a_amount"></param>
    /// <param name="a_minconf"></param>
    /// <param name="a_comment"></param>
    bool Move(string a_fromAccount, string a_toAccount, float a_amount, int a_minconf = 1, string a_comment = "");

    /// <summary>
    /// Amount is a real and is rounded to the nearest 0.01. Returns the transaction ID if successful.
    /// </summary>
    /// <param name="a_fromAccount"></param>
    /// <param name="a_toAddress"></param>
    /// <param name="a_amount"></param>
    /// <param name="a_minconf"></param>
    /// <param name="a_comment"></param>
    /// <param name="a_commentTo"></param>
    string SendFrom(
      string a_fromAccount, 
      string a_toAddress, 
      float a_amount, 
      int a_minconf = 1, 
      string a_comment = "", 
      string a_commentTo = ""
    );

    /// <summary>
    /// Send coins to address. Returns txid.
    /// </summary>
    /// <param name="a_address"></param>
    /// <param name="a_amount"></param>
    /// <param name="a_comment"></param>
    /// <param name="a_commentTo"></param>
    string SendToAddress(string a_address, float a_amount, string a_comment, string a_commentTo);

    /// <summary>
    /// Sets the account associated with the given address.
    /// </summary>
    /// <param name="a_address"></param>
    /// <param name="a_account"></param>
    void SetAccount(string a_address, string a_account);

    /// <summary>
    /// Generation is limited to [genproclimit] processors, -1 is unlimited.
    /// </summary>
    /// <param name="a_generate">true or false to turn generation on or off</param>
    /// <param name="a_genproclimit"></param>
    void SetGenerate(bool a_generate, int a_genproclimit = 1);

    /// <summary>
    /// Stop bitcoin server.
    /// </summary>
    void Stop();

    /// <summary>
    /// Return information about bitcoinaddress.
    /// </summary>
    /// <param name="a_address"></param>
    JObject ValidateAddress(string a_address);
  }
}
