using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BombExplosionManager
{
    public static List<Bomb> bombs = new(); // List of bomb positions
     
    public static Dictionary<Bomb, List<IHittable>> bombToDotsMap;

    
    public void AssignHittablesToBombs(List<IHittable> targetHittables)
    {
        bombToDotsMap = new Dictionary<Bomb, List<IHittable>>();





        // Initialize the dictionary with bombs as keys
        foreach (Bomb bomb in bombs)
        {
            bombToDotsMap[bomb] = new List<IHittable>();
        }

        // Assign each target dot to the closest bomb
        foreach (IHittable hittable in targetHittables)
        {
            Bomb closestBomb = bombs[0];
            DotsGameObject dotsGameObject = (DotsGameObject)hittable;
            float minDistance = Vector2.Distance(dotsGameObject.transform.position, closestBomb.transform.position);

            foreach (Bomb bomb in bombs)
            {
                float distance = Vector2.Distance(dotsGameObject.transform.position, bomb.transform.position);
                if (distance < minDistance)
                {
                    closestBomb = bomb;
                    minDistance = distance;
                }
            }

            bombToDotsMap[closestBomb].Add(hittable);
        }
    }

   
}
