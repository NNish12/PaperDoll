using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public ItemType type;

    // private Vector2 startPos;

    private void Start()
    {
        // startPos = GetComponent<RectTransform>().anchoredPosition;
    }

    public void OnMouseDown()
    {
        if (type == ItemType.Cream && !GameManager.Instance.creamApplied)
        {
            //анимация взятия крема в место между лицом и кремом
            ItemDragController.Instance.StartDrag(this.gameObject, type);

            //перетаскивание игроком
            //если перпенос на зону лица то
            //Если игрок отпускает палец вне зоны лица — ничего не происходит
            //Если игрок отпускает палец в зоне лица:
            //Запускается анимация «нанесение крема на лицо».
            //Меняется спрайт с девочки с прыщами, на девочку без прыщей
            //После окончания анимации - рука автоматически возвращает крем на место.
            //Возврат в дефолт

        }
        if (GameManager.Instance.creamApplied) return;

        if (type == ItemType.Sponge)
        {
            GameManager.Instance.UseSponge();
            return;
        }

        ItemDragController.Instance.StartDrag(this.gameObject, type);
    }
}

public enum ItemType { Cream, Eyeshadow, Lipstick, Brush, Sponge }