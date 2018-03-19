using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace tcp_server
{
    /// <summary>
    /// a class that holds any function with any keys in different directories, has various methods 
    /// </summary>
    public class RequestHandler
    {
        private Dictionary<String, Object> DictionaryRequests = new Dictionary<String, Object>(); // holds all dictionaries 

        private Dictionary<String, Func<Object>> RequestHolder_1 = new Dictionary<String, Func<Object>>(); // a dictionary when a string is a key and a function with a return value of type Object
        private Dictionary<String, Func<Object, Object>> RequestHolder_2 = new Dictionary<String, Func<Object, Object>>(); // a dictionary when a string is a key and a function with a return value of type Object, Object argument
        private Dictionary<String, Func<Object, Object, Object>> RequestHolder_3 = new Dictionary<String, Func<Object, Object, Object>>(); // a dictionary when a string is a key and a function with a return value of type Object, Object argument
        private Dictionary<String, Func<Object, Object, Object, Object>> RequestHolder_4 = new Dictionary<String, Func<Object, Object, Object, Object>>(); // a dictionary when a string is a key and a function with a return value of type Object, Object argument
        private Dictionary<String, Func<Object, Object, Object, Object, Object>> RequestHolder_5 = new Dictionary<String, Func<Object, Object, Object, Object, Object>>(); // a dictionary when a string is a key and a function with a return value of type Object, Object argument
        private Dictionary<String, Func<Object, Object, Object, Object, Object, Object>> RequestHolder_6 = new Dictionary<String, Func<Object, Object, Object, Object, Object, Object>>(); // a dictionary when a string is a key and a function with a return value of type Object, Object argument
        private Dictionary<String, String> Discriptions = new Dictionary<String, String>(); // holds the description on every function 

        private List<String> ArgKeyNames = new List<String>(); // all the keys that used for functions with arguments
        private List<String> Keys = new List<String>();        // all keys for every function
        private List<String> KeysName = new List<String>();    // the name of a key which the user can write in the CLI
        private String ClassName;                              // server or client holder??!?!?
        /// <summary>
        /// initialzies the class instance
        /// </summary>
        /// <param ClassName="Name_"> which type of host is using the holder, server or client.</param>
        public RequestHandler(String Name_)
        {

            ClassName = Name_;
            Keys.Add("Key_1");
            Keys.Add("Key_2");
            Keys.Add("Key_3");
            Keys.Add("Key_4");
            Keys.Add("Key_5");
            Keys.Add("Key_6");
            DictionaryRequests.Add("Key_1", RequestHolder_1);
            DictionaryRequests.Add("Key_2", RequestHolder_2);
            DictionaryRequests.Add("Key_3", RequestHolder_3);
            DictionaryRequests.Add("Key_4", RequestHolder_4);
            DictionaryRequests.Add("Key_5", RequestHolder_5);
            DictionaryRequests.Add("Key_6", RequestHolder_6);
        }
        /// <summary>
        /// Adds a method/function to a specific dictionary
        /// </summary>
        /// <param name="Name"> name of the function</param>
        /// <param function="RequestMethod">the function</param>
        /// <param about="Discription"></param>
        public void AddMethodOnRequest(String Name, Func<Object> RequestMethod, String Discription)
        {
            Name = Name.ToLower();
            Discriptions.Add(Name, Discription);
            Dictionary<String, Func<Object>> val = DictionaryRequests["Key_1"] as Dictionary<String, Func<Object>>;
            val.Add(Name, RequestMethod);
            KeysName.Add(Name);
            Console.WriteLine("Added Command To (" + ClassName + "):" + Name);
            Thread.Sleep(100);
        }
        // same as the method before, just with different parameters to the stored function
        public void AddMethodOnRequest(String Name, Func<Object, Object> RequestMethod, String Discription)
        {
            Name = Name.ToLower();
            Discriptions.Add(Name, Discription);
            Dictionary<String, Func<Object, Object>> val = DictionaryRequests["Key_2"] as Dictionary<String, Func<Object, Object>>;
            val.Add(Name, RequestMethod);
            KeysName.Add(Name);
            ArgKeyNames.Add(Name);
            Console.WriteLine("Added Command To (" + ClassName + "):" + Name);
            Thread.Sleep(100);
        }
        public void AddMethodOnRequest(String Name, Func<Object, Object, Object> RequestMethod, String Discription)
        {
            Name = Name.ToLower();
            Discriptions.Add(Name, Discription);
            Dictionary<String, Func<Object, Object, Object>> val = DictionaryRequests["Key_3"] as Dictionary<String, Func<Object, Object, Object>>;
            val.Add(Name, RequestMethod);
            KeysName.Add(Name);
            ArgKeyNames.Add(Name);
            Console.WriteLine("Added Command To (" + ClassName + "):" + Name);
            Thread.Sleep(100);
        }
        public void AddMethodOnRequest(String Name, Func<Object, Object, Object, Object> RequestMethod, String Discription)
        {
            Name = Name.ToLower();
            Discriptions.Add(Name, Discription);
            Dictionary<String, Func<Object, Object, Object, Object>> val = DictionaryRequests["Key_4"] as Dictionary<String, Func<Object, Object, Object, Object>>;
            val.Add(Name, RequestMethod);
            KeysName.Add(Name);
            ArgKeyNames.Add(Name);
            Console.WriteLine("Added Command To (" + ClassName + "):" + Name);
            Thread.Sleep(100);
        }
        public void AddMethodOnRequest(String Name, Func<Object, Object, Object, Object, Object> RequestMethod, String Discription)
        {
            Name = Name.ToLower();
            Discriptions.Add(Name, Discription);
            Dictionary<String, Func<Object, Object, Object, Object, Object>> val = DictionaryRequests["Key_5"] as Dictionary<String, Func<Object, Object, Object, Object, Object>>;
            val.Add(Name, RequestMethod);
            KeysName.Add(Name);
            ArgKeyNames.Add(Name);
            Console.WriteLine("Added Command To (" + ClassName + "):" + Name);
            Thread.Sleep(100);
        }
        public void AddMethodOnRequest(String Name, Func<Object, Object, Object, Object, Object, Object> RequestMethod, String Discription)
        {
            Name = Name.ToLower();
            Discriptions.Add(Name, Discription);
            Dictionary<String, Func<Object, Object, Object, Object, Object, Object>> val = DictionaryRequests["Key_6"] as Dictionary<String, Func<Object, Object, Object, Object, Object, Object>>;
            val.Add(Name, RequestMethod);
            KeysName.Add(Name);
            ArgKeyNames.Add(Name);
            Console.WriteLine("Added Command To (" + ClassName + "):" + Name);
            Thread.Sleep(100);
        }

        /// <summary>
        /// tries to execute the function that the user asked for
        /// </summary>
        /// <param name="Name">the name of the function</param>
        /// <returns>returns the return value of the executed function</returns>
        public Object TryRequestByName(String Name)
        {
            try
            {
                Dictionary<String, Func<Object>> val = DictionaryRequests["Key_1"] as Dictionary<String, Func<Object>>;
                var method = val[Name];
                return method();
            }
            catch (KeyNotFoundException)
            {
                return "Invalid command";
            }
        }
        public Object TryRequestByName(String Name, Object Arg1)
        {
            try
            {
                Dictionary<String, Func<Object, Object>> val = DictionaryRequests["Key_2"] as Dictionary<String, Func<Object, Object>>;
                var method = val[Name];
                return method(Arg1);
            }
            catch (KeyNotFoundException)
            {
                return "Invalid command";
            }
        }
        public Object TryRequestByName(String Name, Object Arg1, Object Arg2)
        {
            try
            {
                Dictionary<String, Func<Object, Object, Object>> val = DictionaryRequests["Key_3"] as Dictionary<String, Func<Object, Object, Object>>;
                var method = val[Name];
                return method(Arg1, Arg2);
            }
            catch (KeyNotFoundException)
            {
                return "Invalid command";
            }
        }
        public Object TryRequestByName(String Name, Object Arg1, Object Arg2, Object Arg3)
        {
            try
            {
                Dictionary<String, Func<Object, Object, Object, Object>> val = DictionaryRequests["Key_4"] as Dictionary<String, Func<Object, Object, Object, Object>>;
                var method = val[Name];
                return method(Arg1, Arg2, Arg3);
            }
            catch (KeyNotFoundException)
            {
                return "Invalid command";
            }
        }
        public Object TryRequestByName(String Name, Object Arg1, Object Arg2, Object Arg3, Object Arg4)
        {
            try
            {
                Dictionary<String, Func<Object, Object, Object, Object, Object>> val = DictionaryRequests["Key_5"] as Dictionary<String, Func<Object, Object, Object, Object, Object>>;
                var method = val[Name];
                return method(Arg1, Arg2, Arg3, Arg4);
            }
            catch (KeyNotFoundException)
            {
                return "Invalid command";
            }
        }
        public Object TryRequestByName(String Name, Object Arg1, Object Arg2, Object Arg3, Object Arg4, Object Arg5)
        {
            try
            {
                Dictionary<String, Func<Object, Object, Object, Object, Object, Object>> val = DictionaryRequests["Key_6"] as Dictionary<String, Func<Object, Object, Object, Object, Object, Object>>;
                var method = val[Name];
                return method(Arg1, Arg2, Arg3, Arg4, Arg5);
            }
            catch (KeyNotFoundException)
            {
                return "Invalid command";
            }
        }

        /// <summary>
        /// gets all the information about all functions
        /// </summary>
        /// <returns>returns all the function discpritions as a string</returns>
        public String GetAllRequest()
        {

            String AllRequests = "\nCommands:\n";
            int longestlength = 0;
            
            foreach (String Key in KeysName)
            {
                if(Key.Length > longestlength)
                {
                    longestlength = Key.Length;
                }
            }
            int spacebars = longestlength;
            foreach(String Key in KeysName)
            {
                AllRequests += Key;
                int keyspacelength = longestlength - Key.Length + 2;
                int i = 1;
                while (i <= keyspacelength)
                {
                    AllRequests += "-";
                    i += 1;
                }
                
                AllRequests += "-:::";
                AllRequests += Discriptions[Key];
                AllRequests += "\n";
            }
            return AllRequests;

        }
        /// <summary>
        /// checks wheter a function has a argument or not
        /// </summary>
        /// <param name="RequestName">the function that you are searching for</param>
        /// <returns>true of false (has arguments or not)</returns>
        public Boolean HasArg(String RequestName)
        {
            foreach (String Key in ArgKeyNames)
            {
                if (RequestName == Key)
                {
                    return true;
                }
            }
            return false;

        }

    }

}
