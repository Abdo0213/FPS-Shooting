using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float  ammo;
    public float health;
    public int level;
    public Vector3 playerPos;
    public SeriializableDictionary<string, bool> enemyKilled;
    
    public GameData(){
        this.ammo = 50;
        this.health = 200;
        this.level = 1;
        this.playerPos = new Vector3(110f,1.25f,33.7f);
        this.enemyKilled = new SeriializableDictionary<string, bool>();
    }
}
