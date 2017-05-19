using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{

    int verticalRayCount = 4;
    int horizontalRayCount = 5;
    float skinWidth = 0.015f;
    float verticalRaySpacing, horizontalRaySpacing;
    BoxCollider2D myCollider;
    RaycastOrigins raycastOrigins;
    int layerMask;
    public CollisionInfo collisionInfo;

    void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
        layerMask = 1 << LayerMask.NameToLayer("Collision");
        CalculateRaySpacing();
    }

    void Update()
    {
        CalculateRaycastOrigins();
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = myCollider.bounds;
        verticalRaySpacing = (bounds.size.x - 2 * skinWidth) / (verticalRayCount - 1);
        horizontalRaySpacing = (bounds.size.y - 2 * skinWidth) / (horizontalRayCount - 1);
    }

    void CalculateRaycastOrigins()
    {
        Bounds bounds = myCollider.bounds;
        raycastOrigins.bottomLeft = new Vector2(bounds.min.x + skinWidth, bounds.min.y + skinWidth);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x - skinWidth, bounds.min.y + skinWidth);
        raycastOrigins.topLeft = new Vector2(bounds.min.x + skinWidth, bounds.max.y - skinWidth);
        raycastOrigins.topRight = new Vector2(bounds.max.x - skinWidth, bounds.max.y - skinWidth);
    }

    public void Move(Vector2 deltaMovement)
    {
        
        collisionInfo.Reset();
        if (deltaMovement.y != 0)
            HandleVerticalMovement(ref deltaMovement);
        if (deltaMovement.x != 0)
            HandleHorizontalMovement(ref deltaMovement);

        //if (collisionInfo.above || collisionInfo.below)
        //{
        //    deltaMovement.y = 0;
        //}

        transform.Translate(deltaMovement);
    }

    void HandleVerticalMovement(ref Vector2 deltaMovement)
    {
        bool isFalling = deltaMovement.y < 0;
        float raycastDistance = Mathf.Abs(deltaMovement.y) + skinWidth;
        Vector2 rayOrigin = isFalling ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
        Vector2 direction = isFalling ? Vector2.down : Vector2.up;
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayVector = new Vector2(rayOrigin.x + i * verticalRaySpacing, rayOrigin.y);
            Debug.DrawRay(rayVector, direction * raycastDistance, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(rayVector, direction, raycastDistance, layerMask);
            if (hit)
            {
                deltaMovement.y = hit.point.y - rayVector.y;
                raycastDistance = Mathf.Abs(deltaMovement.y) + skinWidth;
                if (isFalling)
                {
                    deltaMovement.y += skinWidth;
                    collisionInfo.below = true;
                }
                else
                {
                    deltaMovement.y -= skinWidth;
                    collisionInfo.above = true;
                }
            }
        }
    }

    void HandleHorizontalMovement(ref Vector2 deltaMovement)
    {
        bool isMovingRight = deltaMovement.x > 0;
        float raycastDistance = Mathf.Abs(deltaMovement.x) + skinWidth;
        Vector2 rayOrigin = isMovingRight ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        Vector2 direction = isMovingRight ? Vector2.right : Vector2.left;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayVector = new Vector2(rayOrigin.x, rayOrigin.y + i * horizontalRaySpacing);
            Debug.DrawRay(rayVector, direction * raycastDistance, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(rayVector, direction, raycastDistance, layerMask);

            if (hit)
            {
                deltaMovement.x = hit.point.x - rayVector.x;
                raycastDistance = Mathf.Abs(deltaMovement.x) + skinWidth;
                if (isMovingRight) deltaMovement.x -= skinWidth;
                else deltaMovement.x += skinWidth;
            }
        }
    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight, bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below, left, right;
        public void Reset()
        {
            above = below = left = right = false;
        }
    }
}