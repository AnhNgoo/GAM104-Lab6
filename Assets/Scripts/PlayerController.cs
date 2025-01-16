using UnityEngine;

public class PlayerController : MonoBehaviour
{  
    [SerializeField]public float moveSpeed = 5f; // Tốc độ di chuyển của nhân vật  

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // Phím A/D hoặc mũi tên trái/phải
        float moveY = Input.GetAxis("Vertical");   // Phím W/S hoặc mũi tên lên/xuống

        Vector2 movement = new Vector2(moveX, moveY);
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }
}