using System.Collections.Generic;
using ObservableCollections;
using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using ProjectOlog.Code.UI.HUD.Killbar.Elements;
using ProjectOlog.Code.UI.HUD.Killbar.Models;
using ProjectOlog.Code.UI.HUD.Killbar.Presenter;
using R3;
using UnityEngine;
using UnityEngine.UIElements;
using TextElement = ProjectOlog.Code.UI.HUD.Killbar.Elements.TextElement;

namespace ProjectOlog.Code.UI.HUD.Killbar.View
{
    public class KillbarView : UIToolkitScreen<KillbarViewModel>
    {
        private VisualElement _killbarContainer;
        private readonly List<VisualElement> _killItemElements = new List<VisualElement>();
        
        protected override void SetVisualElements()
        {
            _killbarContainer = _root.Q<VisualElement>("killbar-container");
            
            // Очищаем контейнер от примеров (если они есть)
            var items = _root.Query<VisualElement>("kill-item").ToList();
            foreach (var item in items)
            {
                if (item.parent != null)
                {
                    item.style.display = DisplayStyle.None;
                }
            }
        }
        
        protected override void OnBind(KillbarViewModel model)
        {
            BindKillItems(model);
        }
        
        protected override void OnUnbind(KillbarViewModel model)
        {
            ClearItems();
        }
        
        private void BindKillItems(KillbarViewModel model)
        {
            model.KillItems.ObserveAdd()
                .Subscribe(addEvent => AddKillItemToUI(addEvent.Value));
            
            model.KillItems.ObserveRemove()
                .Subscribe(removeEvent => RemoveKillItemFromUI(removeEvent.Index));
            
            foreach (var item in model.KillItems)
            {
                AddKillItemToUI(item);
            }
        }
        
        private void AddKillItemToUI(KillbarItemModel itemModel)
        {
            var itemElement = CreateKillItemElement(itemModel);
            _killbarContainer.Add(itemElement);
            _killItemElements.Add(itemElement);
            
            itemModel.LifeTime
                .Subscribe(lifeTime =>
                {
                    bool isVisible = lifeTime > 0f;
                    itemElement.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
                });
        }

        private VisualElement CreateKillItemElement(KillbarItemModel itemModel)
        {
            // Создаем новый элемент
            var itemElement = new VisualElement();
            itemElement.name = "kill-item";

            // Базовые стили для элемента строго по UXML
            itemElement.style.maxHeight = 90;
            itemElement.style.maxWidth = 450;
            itemElement.style.minHeight = 20;
            itemElement.style.flexDirection = FlexDirection.Row;
            itemElement.style.paddingLeft = 12;
            itemElement.style.paddingRight = 12;
            itemElement.style.paddingTop = 1;
            itemElement.style.paddingBottom = 1;
            itemElement.style.marginTop = 0;
            itemElement.style.marginBottom = 3;
            itemElement.style.borderTopLeftRadius = 6;
            itemElement.style.borderTopRightRadius = 6;
            itemElement.style.borderBottomLeftRadius = 6;
            itemElement.style.borderBottomRightRadius = 6;
            itemElement.style.borderLeftWidth = 1;
            itemElement.style.borderRightWidth = 1;
            itemElement.style.borderTopWidth = 1;
            itemElement.style.borderBottomWidth = 1;
            itemElement.style.borderLeftColor = Color.black;
            itemElement.style.borderRightColor = Color.black;
            itemElement.style.borderTopColor = Color.black;
            itemElement.style.borderBottomColor = Color.black;
            itemElement.style.alignSelf = Align.Auto;
            itemElement.style.justifyContent = Justify.FlexStart;
            itemElement.style.alignItems = Align.Center;
            itemElement.style.display = DisplayStyle.Flex; // Важно для UIToolkit

            // Устанавливаем цвет фона и текста в зависимости от типа записи
            if (itemModel.Type == KillbarItemType.Local)
            {
                // Оранжевый для локального игрока
                itemElement.style.backgroundColor = new Color(242f / 255f, 157f / 255f, 32f / 255f);
                itemElement.style.color = Color.black; // Важно задать цвет контейнера
            }
            else
            {
                // Темно-серый для других
                itemElement.style.backgroundColor = new Color(41f / 255f, 37f / 255f, 35f / 255f, 0.9f);
                itemElement.style.color = Color.white; // Важно задать цвет контейнера
            }

            // Добавляем все элементы в полоску
            foreach (var element in itemModel.Elements)
            {
                VisualElement uiElement = null;

                if (element is TextElement textElement)
                {
                    if (itemModel.Type == KillbarItemType.Local)
                    {
                        textElement.TextColor = Color.black;
                    }
                    
                    // Создаем текстовый элемент строго по UXML
                    var label = new Label(textElement.Text);
                    label.style.marginTop = 0;
                    label.style.marginRight = textElement.MarginRight;
                    label.style.marginBottom = 0;
                    label.style.marginLeft = textElement.MarginLeft;
                    label.style.paddingTop = 0;
                    label.style.paddingRight = 1;
                    label.style.paddingBottom = 0;
                    label.style.paddingLeft = 1;
                    label.style.backgroundColor = new Color(0, 0, 0, 0);
                    label.style.color = textElement.TextColor;
                    label.style.unityFontDefinition = new StyleFontDefinition(Resources.Load<Font>("GUI/fonts/tahoma"));
                    label.style.fontSize = textElement.FontSize;
                    label.style.unityTextAlign = TextAnchor.MiddleLeft;
                    label.style.height = StyleKeyword.Auto;
                    label.style.alignSelf = Align.Center;
                    label.style.flexWrap = Wrap.Wrap;

                    // Применяем стили форматирования
                    FontStyle fontStyle = FontStyle.Normal;
                    if (textElement.IsBold && textElement.IsItalic)
                        fontStyle = FontStyle.BoldAndItalic;
                    else if (textElement.IsBold)
                        fontStyle = FontStyle.Bold;
                    else if (textElement.IsItalic)
                        fontStyle = FontStyle.Italic;

                    label.style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(fontStyle);

                    uiElement = label;
                }
                else if (element is ImageElement imageElement)
                {
                    // Создаем элемент-изображение строго по UXML
                    var icon = new VisualElement();
                    icon.style.flexGrow = imageElement.FlexGrow ? 1 : 0;
                    icon.style.flexShrink = 1;
                    icon.style.maxHeight = 25;
                    icon.style.height = imageElement.Height;
                    icon.style.minWidth = StyleKeyword.Auto;
                    icon.style.width = imageElement.Width;
                    icon.style.backgroundImage = Resources.Load<Texture2D>(imageElement.ImagePath);
                    icon.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                    icon.style.paddingRight = 0;
                    icon.style.marginLeft = imageElement.MarginLeft;
                    icon.style.marginRight = imageElement.MarginRight;
                    icon.style.display = DisplayStyle.Flex;
    
                    uiElement = icon;
                }

                if (uiElement != null)
                {
                    itemElement.Add(uiElement);
                }
            }

            return itemElement;
        }

        private void RemoveKillItemFromUI(int index)
        {
            if (index < 0 || index >= _killItemElements.Count) return;
            
            var element = _killItemElements[index];
            _killbarContainer.Remove(element);
            _killItemElements.RemoveAt(index);
        }
        
        private void ClearItems()
        {
            foreach (var itemElement in _killItemElements)
            {
                _killbarContainer.Remove(itemElement);
            }
            
            _killItemElements.Clear();
        }
    }
}