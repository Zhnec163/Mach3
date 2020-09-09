using UnityEngine;
using UnityEngine.EventSystems;

public class Ball : MonoBehaviour
{
    public Color color { get; private set; }
    public SpriteRenderer _spriteRenderer;

    private Animator _animator;
    private Game _game;

    private float _positionX; 
    private float _positionY;
    private int _indexI;
    private int _indexJ;
    private bool movement;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _game = Init.Game();
    }
    private void Update()
    {
        if (movement)
        {
            Motion();
        }
    }
    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            _game.Action(_indexI, _indexJ, color);
        }
    }
    public override string ToString()
    {
        return "I: " + _indexI.ToString() + "J: " + _indexJ.ToString() + " \n \t   " + "X: " + _positionX.ToString() + "Y: " + _positionY.ToString();
    }
    public void SetRandomColor()
    {
        int random = Random.Range(0, 6);

        if (random == 0) { _spriteRenderer.color = Color.blue; color = _spriteRenderer.color; }
        else if (random == 1) { _spriteRenderer.color = Color.red; color = _spriteRenderer.color; }
        else if (random == 2) { _spriteRenderer.color = Color.green; color = _spriteRenderer.color; }
        else if (random == 3) { _spriteRenderer.color = Color.yellow; color = _spriteRenderer.color; }
        else if (random == 4) { _spriteRenderer.color = Color.gray; color = _spriteRenderer.color; }
        else if (random == 5) { _spriteRenderer.color = Color.cyan; color = _spriteRenderer.color; }
    }
    public void AnimationCollapse() => _animator.Play("Collapse");
    public void AnimationFlashing() => _animator.Play("Flashing");
    public void SetCoordinates() 
    {
        _positionX = transform.localPosition.x;
        _positionY = transform.localPosition.y;
    }
    public int GetIndexI() { return _indexI; }
    public int GetIndexJ() { return _indexJ; }
    public void SetIndexI(int indexI) { if (indexI >= 0 && indexI <= Constants.BallsInHeight - 1) _indexI = indexI; }
    public void SetIndexJ(int indexJ) { if (indexJ >= 0 && indexJ <= Constants.BallsInWidth - 1) _indexJ = indexJ; }
    public float GetPositionX() { return _positionX; }
    public float GetPositionY() { return _positionY; }
    public void SetPositionX(float positionX) { if (positionX >= 0 && positionX <= Constants.BallsInWidth - 1) _positionX = positionX; }
    public void SetPositionY(float positionY) { if (positionY <= 0 && positionY >= -(Constants.BallsInHeight - 1)) _positionY = positionY; }
    public bool GetMovement() { return movement; }
    public void MotionBegin() => movement = true;
    private void Motion()
    {
        if (transform.localPosition.y != _positionY)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(transform.localPosition.x, _positionY), 0.05f);
        }
        else movement = false;
    }
}
