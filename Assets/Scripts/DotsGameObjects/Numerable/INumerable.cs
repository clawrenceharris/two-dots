﻿using System.Collections;
using UnityEngine;
public interface INumerable 
{
    int CurrentNumber { get; }
    int InitialNumber { get;  set; }

    int TempNumber { get; set; }

    IEnumerator UpdateCurrentNumber(int number);
   
}