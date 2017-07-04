using UnityEngine;

public interface IDestroyable
{
    void Destruction();
}

public interface IDamagable {
    void TakeHit(float dmg, RaycastHit hit);
    void TakeDamage(float dmg);
}

public interface IHealable<T>
{
    void Heal(T amount);
}

public interface IMoveable
{
    float speed { get; set; }
    void Move();
}

public interface ISwipeable
{
    void Swipe();
    void ReadSwipe(Vector3 _Swipe);
    void TakeActionL();
    void TakeActionR();
    void TakeActionU();
    void TakeActionD();
}

//public void Swipe()
//{
//    Vector3 startPos, endPos, currentSwipe;
//    startPos = endPos = currentSwipe = Vector3.zero;

//#if UNITY_EDITOR
//    if (Input.GetMouseButtonDown(0))
//    {
//        //save began touch 2d point
//        startPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
//    }
//    if (Input.GetMouseButtonUp(0))
//    {
//        //save ended touch 2d point | create vector from the two point
//        endPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
//        currentSwipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

//        //normalize the 2d vector | Pass normalized Vector
//        currentSwipe.Normalize();
//        ReadSwipe(currentSwipe);
//    }
//#elif UNITY_ANDROID
//        if (Input.touches.Length > 0)
//        {
//            Touch t = Input.GetTouch(0);
//            if (t.phase == TouchPhase.Began)
//            {
//                //save began touch 2d point
//                startPos = new Vector2(t.position.x, t.position.y);
//            }
//            if (t.phase == TouchPhase.Ended)
//            {
//                //save ended touch 2d point | create vector from the two point
//                endPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
//                currentSwipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

//                //normalize the 2d vector | Pass normalized Vector
//                currentSwipe.Normalize();
//                ReadSwipe(currentSwipe);
//            }
//        }
//#endif
//}

//public void ReadSwipe(Vector3 _swipe)
//{
//    //swipe upwards
//    if (_swipe.y > 0 && _swipe.x > -0.5f && _swipe.x < 0.5f) { TakeActionU(); }
//    //swipe down
//    else if (_swipe.y < 0 && _swipe.x > -0.5f && _swipe.x < 0.5f) { TakeActionD(); }
//    //swipe left
//    else if (_swipe.x < 0 && _swipe.y > -0.5f && _swipe.y < 0.5f) { TakeActionL(); }
//    //swipe right
//    else if (_swipe.x > 0 && _swipe.y > -0.5f && _swipe.y < 0.5f) { TakeActionR(); }
//}