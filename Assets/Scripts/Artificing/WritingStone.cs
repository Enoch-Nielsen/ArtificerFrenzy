using System;
using System.Collections;
using System.Collections.Generic;
using Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Artificing
{
    public class WritingStone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public enum Symbol
        {
            Fire, Wind, Water, Light, Heavy, Balanced, Common, Rare, Legendary, Imbue, Modify, Temper, None
        }
        
        [Header("References")]
        [SerializeField] private SpriteRenderer symbolSpriteRenderer;
        [SerializeField] private List<Symbol> activeTabletSymbols;

        [Header("Fields")] 
        [SerializeField] private float hoverDistance;
        [SerializeField] private AnimationCurve opacityCurve;
        [SerializeField] private float symbolSwitchSpeed;
        [SerializeField] private List<Sprite> symbolTextures;
        [SerializeField] private Symbol currentSymbol = Symbol.Fire;
        private Dictionary<Symbol, Sprite> symbolTextureDictionary = new Dictionary<Symbol, Sprite>();

        [Header("Values")] [SerializeField] private float targetOpacity;
        [SerializeField] private float currentOpacity;
        [SerializeField] private int currentSymbolLocalIndex;
        [SerializeField] private bool isHovered;
        private Vector3 basePositon;
        private Vector3 targetPosition;

        private void Awake()
        {
            int i = 0;
            foreach (var symbol in Enum.GetValues(typeof(Symbol)))
            {
                symbolTextureDictionary.Add((Symbol)symbol, symbolTextures[i]);
                i++;
            }
        }

        private void Start()
        {
            InputController.Instance.OnSelectPressedAction += OnClick;

            basePositon = transform.position;
            targetPosition = basePositon;

            currentOpacity = 1f;
            targetOpacity = 1f;
        }

        private void Update()
        {
            if (currentOpacity >= targetOpacity && targetOpacity == 0f)
            {
                currentOpacity -= (Time.deltaTime / (symbolSwitchSpeed / 2));
            }
            else if (currentOpacity <= targetOpacity && targetOpacity == 1f)
            {
                currentOpacity += (Time.deltaTime / (symbolSwitchSpeed / 2));
            }
            
            transform.position = Vector3.Lerp(transform.position, targetPosition, 3f * Time.deltaTime);

            Color a = symbolSpriteRenderer.color;
            a.a = currentOpacity;

            symbolSpriteRenderer.color = a;
        }

        private void OnClick(object sender, EventArgs eventArgs)
        {
            if (!isHovered)
                return;

            if (activeTabletSymbols.Count == 0)
                return;
            
            IncrementSymbol();
        }
        
        public void IncrementSymbol()
        {
            if (currentSymbolLocalIndex < activeTabletSymbols.Count-1)
                currentSymbolLocalIndex++;
            else
                currentSymbolLocalIndex = 0;
            
            currentSymbol = activeTabletSymbols[currentSymbolLocalIndex];
            targetOpacity = 0f;

            StartCoroutine(UpdateSymbolVisual(symbolSwitchSpeed / 2f));
        }
        
        private IEnumerator UpdateSymbolVisual(float time)
        {
            yield return new WaitForSeconds(time);
            symbolSpriteRenderer.sprite = symbolTextureDictionary[currentSymbol];
            targetOpacity = 1f;
        }

        public Symbol GetCurrentSymbol()
        {
            return currentSymbol;
        }

        public void Reset()
        {
            currentSymbol = Symbol.None;
            currentSymbolLocalIndex = 0;
            
            currentSymbol = activeTabletSymbols[currentSymbolLocalIndex];
            targetOpacity = 0f;

            StartCoroutine(UpdateSymbolVisual(symbolSwitchSpeed / 2f));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (activeTabletSymbols.Count == 0)
                return;

            targetPosition = basePositon + (transform.up * hoverDistance);
            
            isHovered = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (activeTabletSymbols.Count == 0)
                return;

            targetPosition = basePositon;
            
            isHovered = false;
        }
    }
}