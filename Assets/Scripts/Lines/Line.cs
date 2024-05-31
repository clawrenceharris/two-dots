using UnityEngine;
public class ConnectorLine : MonoBehaviour
{
    public Vector2 endPos;
    public Vector2 startPos;
    public Color color;
    public bool disabled;
    public LineRenderer lineRenderer;
    public Vector2 initialScale; // Initial scale of the line

    public Quaternion rotation = Quaternion.identity;
    public SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        SetUp();
    }
    
    
    
    void SetUp()
    {
        initialScale = transform.localScale;
    }
    void Update()
    {
        if(!disabled)
            UpdateLine();
       
        
    }
    void UpdateLine()
    {
        sprite.color = color;
        float distance = Vector2.Distance(startPos, endPos);
        

        // Calculate the new scale value based on the distance
        float newXScale = initialScale.x + distance; // Adjust this value as per your requirement


        // Calculate the new position based on the angle between the start position and the end position
        float angle = Vector2.SignedAngle(Vector2.right, endPos - startPos);
        float newPositionX = startPos.x + Mathf.Cos(angle * Mathf.Deg2Rad) * distance * 0.5f; // Adjust this if your pivot point is different
        float newPositionY = startPos.y + Mathf.Sin(angle * Mathf.Deg2Rad) * distance * 0.5f; // Adjust this if your pivot point is different

       
        
         transform.rotation = Quaternion.Euler(0f, 0f, angle);

        

        // Apply the new scale and position to the object
        transform.localScale = new Vector3(newXScale, initialScale.y);
        transform.position = new Vector3(newPositionX, newPositionY);
    
   
    }
}
