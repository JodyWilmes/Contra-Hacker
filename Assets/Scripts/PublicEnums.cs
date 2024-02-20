using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PublicEnums : MonoBehaviour
{

}

public class GameObjectEvent : UnityEvent<GameObject>
{
}
public enum RedFlag { wrongSender, wrongReceiver, wrongSubject, wrongBody, wrongLink, wrongAttachment, wrongLogo }
public enum MailHandleType { clickDelete,clickAttachment,clickLink,clickForward}

public enum BuyableTrinkets
{
    rubberDuck,
    newtonPendel,
    goldDuck,
}