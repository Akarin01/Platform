using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class CharacterEvents
{
    public static UnityEvent<GameObject, int> onCharacterDameged = new UnityEvent<GameObject, int>();
    public static UnityEvent<GameObject, int> onCharacterHealed = new UnityEvent<GameObject, int>();
}
