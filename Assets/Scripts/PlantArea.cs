using UnityEngine;

public class PlantArea : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collider){
        if(collider.gameObject.CompareTag("TerroDirt")){
            collider.GetComponent<TerroDirt>().canPlant = true;
        }
    }
    void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.CompareTag("TerroDirt")){
            collider.GetComponent<TerroDirt>().canPlant = false;
        }
    }
}
