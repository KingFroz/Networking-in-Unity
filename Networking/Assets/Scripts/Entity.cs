
using UnityEngine;

public abstract class Entity : MonoBehaviour, IMoveable, ISwipeable
{
    //Movement
    public float speed { get; set;}
    public virtual void InitMovement() { speed = 2.0f; }
    public abstract void Move();
    //Swipe Commands
    public abstract void TakeActionL();
    public abstract void TakeActionR();
    public abstract void TakeActionU();
    public abstract void TakeActionD();

    public void Swipe()
    {
        Vector3 startPos, endPos, currentSwipe;
        startPos = endPos = currentSwipe = Vector3.zero;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            //save began touch 2d point
            startPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Input.GetMouseButtonUp(0))
        {
            //save ended touch 2d point | create vector from the two point
            endPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            currentSwipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

            //normalize the 2d vector | Pass normalized Vector
            currentSwipe.Normalize();
            ReadSwipe(currentSwipe);
        }
#elif UNITY_ANDROID
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                //save began touch 2d point
                startPos = new Vector2(t.position.x, t.position.y);
            }
            if (t.phase == TouchPhase.Ended)
            {
                //save ended touch 2d point | create vector from the two point
                endPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                currentSwipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

                //normalize the 2d vector | Pass normalized Vector
                currentSwipe.Normalize();
                ReadSwipe(currentSwipe);
            }
        }
#endif
    }

    public void ReadSwipe(Vector3 _swipe)
    {
        //swipe upwards
        if (_swipe.y > 0 && _swipe.x > -0.5f && _swipe.x < 0.5f) { TakeActionU(); }
        //swipe down
        else if (_swipe.y < 0 && _swipe.x > -0.5f && _swipe.x < 0.5f) { TakeActionD(); }
        //swipe left
        else if (_swipe.x < 0 && _swipe.y > -0.5f && _swipe.y < 0.5f) { TakeActionL(); }
        //swipe right
        else if (_swipe.x > 0 && _swipe.y > -0.5f && _swipe.y < 0.5f) { TakeActionR(); }
    }
}
