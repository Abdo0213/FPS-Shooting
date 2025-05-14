using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SeriializableDictionary<Tkey, Tvalue> : Dictionary<Tkey, Tvalue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<Tkey> keys = new List<Tkey>();
    [SerializeField] private List<Tvalue> values = new List<Tvalue>();
    public void OnBeforeSerialize(){
        keys.Clear();
        values.Clear();
        foreach(KeyValuePair<Tkey, Tvalue> pair in this){
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }
    public void OnAfterDeserialize(){
        this.Clear();
        if(keys.Count != values.Count){
            Debug.LogError("Tried to deserialize a serializebleDictionary, but the ammount of keys(" + keys.Count + ") does not match the number of values (" + ")which indicates that something is wrong.");
        }
        for(int i = 0; i < keys.Count; i++){
            this.Add(keys[i], values[i]);
        }
    }
}
