using UnityEngine;

public interface IDestroyable
{
    void Destruction();
}

public interface IDamagable<T>
{
    void Damage(T damage);
}

public interface IHealable<T>
{
    void Heal(T amount);
}

public interface IMoveable
{
    float speed { get; set; }
    void InitMovement();
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