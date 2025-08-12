# Paper Doll
Unity version: 2022.3.62f1 (LTS)


Игрок наносит макияж на персонажа с помощью простого и интуитивного управления.  
Игровой процесс построен на перетаскивании предмета с выбранным косметическим средством (крем, тени, помада) в зону лица персонажа.  

---


## Прототип
![Превью симуляции](Assets/gifs/prototype.gif)


---

## Особенности

- **Спонж (Sponge)** — одним нажатием стирает весь макияж c визуальным эффектом.
- **Particle System** — простая анимация пузырей для атмосферности.
- **Плавная смена прозрачности** — корутины для аккуратного исчезновения и появления спрайтов.
- **Логика перетаскивания** — предмет возвращается на место, если не попал в зону лица.

---

## Основные скрипты и их функции

### 1. `InteractableObject.cs`
- Обрабатывает интерактивное поведение предметов (крем, тени, помада, кисти, спонж).
- Реализует события: нажатие, начало перетаскивания, перетаскивание, окончание перетаскивания.
```csharp
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (FaceZone.IsOverZone(eventData.position))
        {
            isInteractive = false;
            ItemAnimator.Instance.HandleDropAction(itemType, this.gameObject);
        }
        else
        {
            rectTransform.anchoredPosition = startPos;
        }
    }

```

### 2. `ItemAnimator.cs`
- Отвечает за анимации перемещений, масштабирования и эффектов.
- Управляет логикой применения косметики.
- Использует корутины для плавных анимаций.
- Взаимодействует с `GirlManager` для изменения внешности персонажа.

### Логика применения перетаскиваемого премета, через enum и делегат

```csharp
    public void HandleDropAction(ItemType type, GameObject item)
    {
        tool = item.GetComponent<RectTransform>();
        currentType = type;
        switch (type)
        {
            case ItemType.Cream:

                if (!gameManager.creamApplied)
                    StartCoroutine(ApplyCream(item));
                break;

            case ItemType.Eyeshadow:
                if (tool == null) return;
                StartCoroutine(ApplyAndUnlock(() => girlManager.ApplyShadow(MakeupManager.Instance.selectedSprite)));
                break;

            case ItemType.Brush:
                if (tool == null) return;
                StartCoroutine(ApplyAndUnlock(() => girlManager.ApplyBlush(MakeupManager.Instance.selectedSprite)));
                break;

            case ItemType.Lipstick:
                if (tool == null) return;
                StartCoroutine(ApplyLipstiсAndUnlock(() => girlManager.ApplyLipstick(MakeupManager.Instance.selectedSprite)));
                break;

        }
    }
```


### Перемещение премета, используя корутины

```csharp
    public IEnumerator MoveTo(RectTransform item, RectTransform target = null, Vector2? targetPosition = null, float duration = 1f)
    {
        Vector2 finalTargetPos;

        if (target != null)
        {
            Vector3 worldTargetPos = target.position;
            RectTransform parent = item.parent as RectTransform;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parent,
                RectTransformUtility.WorldToScreenPoint(null, worldTargetPos),
                null,
                out finalTargetPos
            );

            float itemHeight = item.rect.height;
            finalTargetPos.y -= itemHeight / 2f;
        }
        else if (targetPosition.HasValue)
        {
            finalTargetPos = targetPosition.Value;
        }
        else
        {
            yield break;
        }

        Vector2 start = item.anchoredPosition;
        float t = 0f;

        while (t < duration)
        {
            item.anchoredPosition = Vector2.Lerp(start, finalTargetPos, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        item.anchoredPosition = finalTargetPos;
    }
```


### 3. `MakeupManager.cs`
- Отвечает за выбор и применение косметических средств.
- Хранит спрайты и текущие выбранные инструменты.
- Обрабатывает выбор цвета/типа косметики.
- Управляет доступностью палитры через `GameManager`.
- Реализует плавное появление/исчезновение косметики.

```csharp
    public Sprite[] Lips;
    public Sprite[] BrushSprites;
    public Sprite[] EyeshadowSprites;
    public Sprite selectedSprite;
    private ItemType selectedType;
    ...
    public void SetColor(ButtonColor colorButton)
    {
        if (!GameManager.Instance.CanInteractWithPalette) return;

        selectedType = colorButton.itemType;
        button = colorButton.GetComponent<RectTransform>();

        switch (selectedType)
        {
            case ItemType.Eyeshadow:
                selectedSprite = EyeshadowSprites[colorButton.index];
                ItemAnimator.Instance.PlayToolToFace(eyebrush);
                break;

            case ItemType.Brush:
                selectedSprite = BrushSprites[colorButton.index];
                ItemAnimator.Instance.PlayToolToFace(brush);
                break;

            case ItemType.Lipstick:
                currentLipstic = button.GetComponent<RectTransform>();
                selectedSprite = Lips[colorButton.index];
                ItemAnimator.Instance.PlayLipstic(currentLipstic);
                break;
        }
        GameManager.Instance.CanInteractWithPalette = false;
    }
```

### 4. `ButtonColor.cs`
- Логика кнопок выбора цвета и типа косметики.
- Передает данные о выборе в `MakeupManager`.

### 5. `GirlManager.cs`
- Хранит ссылки на `SpriteRenderer` для разных зон лица.
- Проверяет наличие косметики для управления UI.
- Взаимодействует со `SpongeController` для очистки лица.
- Управление прозрачностью при установке спрайтов макияжа.


```csharp
        Color color = sr.color;

        if (sr.sprite != null)
        {
            float startAlpha = color.a;
            float halfDuration = duration / 2f;
            float elapsed = 0f;

            while (elapsed < halfDuration)
            {
                elapsed += Time.deltaTime;
                color.a = Mathf.Lerp(startAlpha, 0f, elapsed / halfDuration);
                sr.color = color;
                yield return null;
            }

            sr.sprite = sprite;
            elapsed = 0f;

            while (elapsed < halfDuration)
            {
                elapsed += Time.deltaTime;
                color.a = Mathf.Lerp(0f, targetAlpha, elapsed / halfDuration);
                sr.color = color;
                yield return null;
            }
            color.a = targetAlpha;
            sr.color = color;
        }
```

### 6. `GameManager.cs`
- Управляет ключевыми игровыми состояниями (например, можно ли взаимодействовать с палитрой, нанесен ли крем).
- Отвечает за сброс макияжа и перезапуск сцены.
- Контролирует доступ к UI через `UIController`.

---

## Взаимодействие между скриптами

1. **`ButtonColor`** передает выбор пользователя в `MakeupManager`.
2. **`InteractableObject`** реагирует на пользовательские действия и проверяет состояния в `GameManager`.
3. При сбросе предмета на лицо вызывается `ItemAnimator.HandleDropAction`.
4. **`ItemAnimator`** запускает анимацию и вызывает методы `GirlManager` для обновления внешности.
5. **`MakeupManager`** управляет выбором косметики и анимациями через `ItemAnimator`.
6. **`GirlManager`** обновляет спрайты лица и влияет на доступность элементов.

