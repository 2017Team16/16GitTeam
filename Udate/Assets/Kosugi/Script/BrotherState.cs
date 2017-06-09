using UnityEngine;
using System.Collections;

public enum BrotherState
{
    NONE = 0,
    NORMAL,     //通常時
    THROW,      //投げられている時  
    BACK,       //兄の元へ戻っている時
    SPECIAL,     //必殺技
    THROWSTART
}
