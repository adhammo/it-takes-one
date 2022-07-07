using UnityEngine;

public class Hurt : MonoBehaviour
{
    private Animator _anim;
    private int _hurtAnimHash = Animator.StringToHash("Hurt");

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void GetHurt()
    {
        _anim.SetTrigger(_hurtAnimHash);
    }
}
