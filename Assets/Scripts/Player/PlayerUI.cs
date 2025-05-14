using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour, IDataPersistence{
    [SerializeField] private TextMeshProUGUI promptMessage;
    [SerializeField] private TextMeshProUGUI ammo;
    void Start()
    {
        
    }
    public void LoadData(GameData data){
        this.ammo.text = data.ammo.ToString();
    }
    public void SaveData(ref GameData data){
        
    }
    public void UpdateText(string promptMessage){
        this.promptMessage.text = promptMessage;
    }
    public void UpdateAmmo(float ammo)
    {
        this.ammo.text = ammo.ToString();
    }
}