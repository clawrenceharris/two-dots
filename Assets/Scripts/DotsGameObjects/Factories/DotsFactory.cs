using UnityEngine;
using System;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using System.Collections.Generic;

public class DotsFactory
{
    public static T CreateDotsGameObject<T>(DotObject dObject)
        where T : DotsGameObject
    {
        
        DotsGameObject dotsGameObject = Object.Instantiate(LevelLoader.FromJsonType(dObject.type));
        if (dotsGameObject is IHittable hittable)
        {
            hittable.HitCount = dObject.hitCount;

        }

        if (dotsGameObject is IColorable colorable)
        {
            string color = dObject.GetProperty<string>(DotObject.Property.Color);
            colorable.Color = LevelLoader.FromJsonColor(color);
        }

        if (dotsGameObject is INumerable numberable)
        {
            int number = dObject.GetProperty<int>(DotObject.Property.Number);

            numberable.InitialNumber = number;
        }

        if (dotsGameObject is IDirectional directional)
        {
            int[,] directions = dObject.GetProperty<int[,]>(DotObject.Property.Directions);
            int row = Random.Range(0, directions.GetLength(0));
            
            directional.DirectionX = directions[row, 0];
            directional.DirectionY = directions[row, 1];
        }

        if(dotsGameObject is IMultiColorable multicolorable)
        {            
            var colors = LevelLoader.Level.colors;

            multicolorable.Colors = new DotColor[3];

            //list that will contain 3 unique colors to initialize the dot object's colors
            List<DotColor> dotColors = new();
            DotColor color = multicolorable.Color;

            for (int i = 0 ; i < 3; i++)
            {
                dotColors.Add(color);
                
                while(true){
                    int rand = Random.Range(0, colors.Length);
                    //get a random color from the level's colors
                    color = LevelLoader.FromJsonColor(colors[rand]);
                    //if the color is not already in the list exit the loop so it can be added
                    if(!dotColors.Contains(color)){
                        break;
                    }
                }


            }

            multicolorable.Colors = dotColors.ToArray();

        }

        if(dotsGameObject is ISwitchable switchable)
        {
            bool isActive = dObject.GetProperty<bool>(DotObject.Property.Active);
            switchable.IsActive = isActive;

           
        }

        return (T)dotsGameObject;
    }

}