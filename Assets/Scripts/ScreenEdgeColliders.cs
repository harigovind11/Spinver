using UnityEngine;

public class ScreenEdgeColliders : MonoBehaviour
{
    public float thickness = 0.1f;

    void Start()
    {
        float screenHeight = 2f * Camera.main.orthographicSize;
        float screenWidth = screenHeight * Camera.main.aspect;

        CreateEdge("Left", "Side", new Vector2(-screenWidth / 2 - thickness / 2, 0), new Vector2(thickness, screenHeight));
        CreateEdge("Right", "Side", new Vector2(screenWidth / 2 + thickness / 2, 0), new Vector2(thickness, screenHeight));
        CreateEdge("Top", "Top", new Vector2(0, screenHeight / 2 + thickness / 2), new Vector2(screenWidth, thickness));
        CreateEdge("Bottom", "Bottom", new Vector2(0, -screenHeight / 2 - thickness / 2), new Vector2(screenWidth, thickness));
    }

    void CreateEdge(string name, string tag, Vector2 center, Vector2 size)
    {
        GameObject edge = new GameObject(name);
        edge.transform.parent = transform;

        edge.transform.position = center;

        BoxCollider2D col = edge.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        col.size = size;

        edge.tag = tag;

        Rigidbody2D rb = edge.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
    }
}
