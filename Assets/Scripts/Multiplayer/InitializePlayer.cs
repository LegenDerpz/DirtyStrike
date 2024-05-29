
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Cinemachine;
using Newtonsoft.Json;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Players {
    public string username {get; set;}
    public string team {get; set;}
    public string room {get; set;}
}

public class InitializePlayer : MonoBehaviour
{

    [SerializeField]
    string API_URL = "http://localhost:3000/rooms/";

        [SerializeField]
    Uri uri = new Uri("http://localhost:7000");

    SocketIOUnity socket;

    GameObject mePlayerObj;
    Vector2 playerPosition;
    float playerRotation;

    string currentUsername;
    string team;
    string room;

    CinemachineVirtualCamera cinemachineVCam;

    public SocketIOUnity Socket { get => socket; }

    async void Awake()
    {        
        
        string resPlayers = await GetPlayers();

        Players[] players = JsonConvert.DeserializeObject<Players[]>(resPlayers).ToArray();
                
        GameObject[] TSpawns = GameObject.FindGameObjectsWithTag("TSpawn");
        GameObject[] PSpawns = GameObject.FindGameObjectsWithTag("PSpawn");

        cinemachineVCam = GameObject.FindGameObjectWithTag("MeCamera").GetComponent<CinemachineVirtualCamera>();

        string meUsername = PlayerPrefs.GetString("username");

        for(int i = 0, p = 0, t = 0; i < players.Length; i++){           
            
            Players player = players[i];
            GameObject playerPrefab;

            if(player.username == meUsername){
                playerPrefab = Resources.Load<GameObject>("Prefabs/Me");
                playerPrefab.name = player.username;
            } else {
                playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
                playerPrefab.name = player.username;
            }

            playerPrefab.tag = player.team;

            if(player.team.Equals("Purifier")){
                int layer = 10;
                GameObject head, rightArm, rightHand, leftArm, leftHand;
                GameObject spriteObject = playerPrefab.transform.GetChild(0).gameObject;
                SpriteResolver sr_head, sr_rightArm, sr_rightHand, sr_leftArm, sr_leftHand;
                

                head = spriteObject.transform.GetChild(0).gameObject;
                rightHand = spriteObject.transform.GetChild(1).gameObject;
                leftHand = spriteObject.transform.GetChild(2).gameObject;
                leftArm = spriteObject.transform.GetChild(3).gameObject;
                rightArm = spriteObject.transform.GetChild(4).gameObject;

                sr_head = head.GetComponent<SpriteResolver>();
                sr_rightArm = rightArm.GetComponent<SpriteResolver>();
                sr_rightHand = rightHand.GetComponent<SpriteResolver>();
                sr_leftArm = leftArm.GetComponent<SpriteResolver>();
                sr_leftHand = leftHand.GetComponent<SpriteResolver>();

                sr_head.SetCategoryAndLabel("Head", "Purifier Head");
                sr_rightArm.SetCategoryAndLabel("R_Arm", "Purifier Right Arm");
                sr_rightHand.SetCategoryAndLabel("R_Hand", "Purifier Right Hand");
                sr_leftArm.SetCategoryAndLabel("L_Arm", "Purifier Left Arm");
                sr_leftHand.SetCategoryAndLabel("L_Hand", "Purifier Left Hand");
                playerPrefab.GetComponent<Purifier>().enabled = true;
                playerPrefab.GetComponent<Combat>().layerMask |= 2 << layer;
                gameObject.layer = 10;

                Instantiate(playerPrefab, PSpawns[p].transform.position, Quaternion.identity);
                p++;
            }
            
            if(player.team.Equals("TerroDirt")){
                int layer = 10;
                GameObject head, rightArm, rightHand, leftArm, leftHand;
                GameObject spriteObject = playerPrefab.transform.GetChild(0).gameObject;
                SpriteResolver sr_head, sr_rightArm, sr_rightHand, sr_leftArm, sr_leftHand;
                
                head = spriteObject.transform.GetChild(0).gameObject;
                rightHand = spriteObject.transform.GetChild(1).gameObject;
                leftHand = spriteObject.transform.GetChild(2).gameObject;
                leftArm = spriteObject.transform.GetChild(3).gameObject;
                rightArm = spriteObject.transform.GetChild(4).gameObject;

                sr_head = head.GetComponent<SpriteResolver>();
                sr_rightArm = rightArm.GetComponent<SpriteResolver>();
                sr_rightHand = rightHand.GetComponent<SpriteResolver>();
                sr_leftArm = leftArm.GetComponent<SpriteResolver>();
                sr_leftHand = leftHand.GetComponent<SpriteResolver>();

                sr_head.SetCategoryAndLabel("Head", "TerroDirt Head");
                sr_rightArm.SetCategoryAndLabel("R_Arm", "TerroDirt Right Arm");
                sr_rightHand.SetCategoryAndLabel("R_Hand", "TerroDirt Right Hand");
                sr_leftArm.SetCategoryAndLabel("L_Arm", "TerroDirt Left Arm");
                sr_leftHand.SetCategoryAndLabel("L_Hand", "TerroDirt Left Hand");
                playerPrefab.GetComponent<TerroDirt>().enabled = true;
                playerPrefab.GetComponent<Combat>().layerMask |= 1 << layer; //Can Only Damage Purifier Players
                gameObject.layer = 11;

                Instantiate(playerPrefab, TSpawns[t].transform.position, Quaternion.identity);
                t++;
            }

        }       

        mePlayerObj = GameObject.Find(meUsername + "(Clone)");
        
        currentUsername = PlayerPrefs.GetString("username");
        team = PlayerPrefs.GetString("selectedTeam");
        room  = PlayerPrefs.GetString("currentJoinedRoom");

        mePlayerObj.name = currentUsername;
        mePlayerObj.tag = team;

        cinemachineVCam.Follow = mePlayerObj.transform;

        socket = new SocketIOUnity(uri, new SocketIOOptions{
            Query = new Dictionary<string, string>
            {
                {"username", currentUsername},
                {"team", team},
                {"room", room}
            }
        });
        
        Socket.Connect();

        this.Socket.JsonSerializer = new NewtonsoftJsonSerializer();

        Socket.On("other_players_transform", (data) => {           
            UnityThread.executeInUpdate(() => {
                    
                    string resPlayerTransform = data.GetValue<string>();
                    PlayerTransform playerTransform = JsonConvert.DeserializeObject<PlayerTransform>(resPlayerTransform);
                    
                    GameObject[] playerTerroDirt = GameObject.FindGameObjectsWithTag("TerroDirt");
                    GameObject[] playerPurifier = GameObject.FindGameObjectsWithTag("Purifier");
                    
                    for (int i = 0; i < playerTerroDirt.Length; i++)
                    {
                        if(playerTerroDirt[i].name.Equals(playerTransform.username + "(Clone)")){
                            playerTerroDirt[i].transform.position = new Vector2(playerTransform.player_position.x, playerTransform.player_position.y);
                            playerTerroDirt[i].GetComponent<Rigidbody2D>().rotation = playerTransform.player_rotation;
                        }
                    }

                    for (int i = 0; i < playerPurifier.Length; i++)
                    {
                        if(playerPurifier[i].name.Equals(playerTransform.username + "(Clone)")){
                            playerPurifier[i].transform.position = new Vector2(playerTransform.player_position.x, playerTransform.player_position.y);
                            playerPurifier[i].GetComponent<Rigidbody2D>().rotation = playerTransform.player_rotation;
                        }
                    }

            });
        });
  
        Socket.On("other_bullet_transform", (data) => {
            UnityThread.executeInUpdate(() => {
                
                string resBulletTransform = data.GetValue<string>();
                BulletTransform bulletTransform = JsonConvert.DeserializeObject<BulletTransform>(resBulletTransform);
                GameObject bulletPrefab = Resources.Load<GameObject>("Prefabs/Water_Bullet");
                bulletPrefab.transform.position = new Vector2(bulletTransform.bullet_position.x, bulletTransform.bullet_position.y);

            });
        });
    
    }

    void Update()
    {

        if(mePlayerObj != null){
            print(mePlayerObj);
            string currentUsername = PlayerPrefs.GetString("username");

            playerPosition = mePlayerObj.GetComponent<Transform>().position;
            playerRotation = mePlayerObj.GetComponent<Rigidbody2D>().rotation;
            
            string playerTransform =  @"{{
                ""username"": ""{0}"",
                ""player_position"": {{""x"": {1}, ""y"": {2}}},
                ""player_rotation"": {3}
                }}";
            
            Socket.Emit("player_transform", string.Format(playerTransform,  currentUsername, playerPosition.x, playerPosition.y, playerRotation));

        }
    }
    
    async Task<string> GetPlayers(){

        string room = PlayerPrefs.GetString("currentJoinedRoom");
        API_URL += room;

        using var httpClient = new HttpClient();

        var res = await httpClient.GetAsync(API_URL);
        return await res.Content.ReadAsStringAsync();
   }

}
