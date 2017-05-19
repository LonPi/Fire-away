using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class CharacterController2D : MonoBehaviour {

    BoxCollider2D myCollider;
    int horizontalRayCount = 4;
    int verticalRayCount = 4;
    int layerMask;
    float verticalRaySpacing, horizontalRaySpacing;
    float skinWidth = 0.02f;

    // raycast points
    Vector2 topLeft, topRight, bottomLeft, bottomRight;
    public State state;

    public struct State
    {
        public bool IsCollidingAbove, IsCollidingBelow, IsCollidingRight, IsCollidingLeft;
        public void Reset()
        {
            IsCollidingAbove = IsCollidingBelow = IsCollidingLeft = IsCollidingRight = false;
        }
    }

    private void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
        layerMask = 1 << LayerMask.NameToLayer("Platform");
        CalculateRaySpacing();
        state.Reset();
    }
    void Update () {
        CalculateRaySpacing();
        UpdateRaycastOrigins();
    }

    void CalculateRaySpacing()
    {
        float boxWidth = (topRight.x - topLeft.x);
        float boxHeight = (topLeft.y - bottomLeft.y);
        verticalRaySpacing = boxWidth / (verticalRayCount - 1);
        horizontalRaySpacing = boxHeight / (horizontalRayCount - 1);
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = myCollider.bounds;
        topLeft = new Vector2(bounds.min.x+skinWidth, bounds.max.y-skinWidth);
        topRight = new Vector2(bounds.max.x-skinWidth, bounds.max.y-skinWidth);
        bottomLeft = new Vector2(bounds.min.x+skinWidth, bounds.min.y+skinWidth);
        bottomRight = new Vector2(bounds.max.x-skinWidth, bounds.min.y+skinWidth);
    }

    public void Move(Vector2 deltaMovement)
    {
        state.Reset();

        if (deltaMovement.y != 0)
        {
            HandleVerticalCollisions(ref deltaMovement);
        }

        if (deltaMovement.x != 0)
        {
            HandleHorizontalCollision(ref deltaMovement);
        }

        transform.Translate(deltaMovement);
    }

    void HandleHorizontalCollision(ref Vector2 deltaMovement)
    {
        bool isMovingRight = deltaMovement.x > 0;
        float rayDistance = Mathf.Abs(deltaMovement.x) + skinWidth;
        Vector2 direction = isMovingRight ? Vector2.right : Vector2.left;
        Vector2 rayOrigin = isMovingRight ? bottomRight : bottomLeft;
        
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayVector = new Vector2(rayOrigin.x, rayOrigin.y + i * horizontalRaySpacing);
            Debug.DrawRay(rayVector, direction * rayDistance, Color.blue);
            RaycastHit2D hit = Physics2D.Raycast(rayVector, direction, rayDistance, layerMask);

            if (!hit) continue;
            if (isMovingRight)
            {
                deltaMovement.x = hit.point.x - rayVector.x - skinWidth;
                state.IsCollidingRight = true;
            }
            else
            {
                deltaMovement.x = hit.point.x - rayVector.x + skinWidth;
                state.IsCollidingLeft = true;
            }
        }
    }

    void HandleVerticalCollisions(ref Vector2 deltaMovement)
    {
        float rayDistance = Mathf.Abs(deltaMovement.y) + skinWidth;
        bool isGoingUp = deltaMovement.y > 0;
        Vector2 rayOrigin = isGoingUp ? topLeft : bottomLeft;
        Vector2 direction = isGoingUp ? Vector2.up : Vector2.down;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayVector = new Vector2(rayOrigin.x + i * verticalRaySpacing, rayOrigin.y);
            Debug.DrawRay(rayVector, direction * rayDistance, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(rayVector, direction, rayDistance, layerMask);

            if (!hit) continue;
            deltaMovement.y = hit.point.y - rayVector.y;
            rayDistance = Mathf.Abs(deltaMovement.y);

            if (isGoingUp)
            {
                deltaMovement.y -= skinWidth;
                state.IsCollidingAbove = true;
            }
            else
            {
                deltaMovement.y += skinWidth;
                state.IsCollidingBelow = true;
            }

        }
    }
}