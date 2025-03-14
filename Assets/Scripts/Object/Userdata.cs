using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class Userdata : MonoBehaviour
{
    public class UserData
    {
        private string _uid;
        private int _reputation;
        // Branch Id -> Advancement
        private Dictionary<string, List<string>> _advancements;
        private List<string> _items;
        private List<string> _scannedCodes;
        private Dictionary<string, bool> _challenges;
        private long _finishTimestamp;
        private long _startTimestamp;

        [Preserve]
        public UserData()
        {
        }

        public UserData(string uid)
        {
            _uid = uid;
            _reputation = 1;
            _advancements = new Dictionary<string, List<string>>();
            _items = new List<string>();
            _finishTimestamp = -1;
            _startTimestamp = -1;
            _scannedCodes = new List<string>();
            _challenges = new Dictionary<string, bool>();
        }
    }
}
