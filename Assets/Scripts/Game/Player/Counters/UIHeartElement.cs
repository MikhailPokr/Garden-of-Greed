using UnityEngine;
using UnityEngine.UI;

namespace Garden
{
    public class UIHeartElement : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private Image _lens;

        private int _maxHp;
        private Color[] _palette;
        private Sprite _normalLens;
        private Sprite _brokenLens;

        private int _hp;

        public void Init(int maxElementHp, Color[] palette, Sprite normalLens, Sprite brokenLens)
        {
            _maxHp = maxElementHp;
            _normalLens = normalLens;
            _brokenLens = brokenLens;
            
            if (palette.Length != _maxHp)
            {
                int size = _maxHp + 1;
                
                _palette = new Color[size];

                for (int i = 0; i < _palette.Length; i++)
                {
                    float t = size > 1 ? (float)i / (size - 1) : 0f;
                    
                    float palettePos = t * (palette.Length - 1);
                    
                    int index1 = (int)palettePos;
                    int index2 = Mathf.Min(index1 + 1, palette.Length - 1);
                    
                    float localT = palettePos - index1;
            
                    _palette[i] = Color.Lerp(palette[index1], palette[index2], localT);
                }
            }
            else
            {
                _palette = palette;
            }
            
            _lens.sprite = _normalLens;
        }

        public int ChangeHp(int hp)
        {
            int remainder = 0;
            if (hp == 0)
            {
                _lens.sprite = _brokenLens;
                _background.color = _palette[0];
                _hp = 0;
                return 0;
            }
            if (_hp == 0)
                _lens.sprite = _normalLens;
            if (hp >= _maxHp)
            {
                remainder = hp - _maxHp;
                _hp = _maxHp;
            }
            else
            {
                _hp = hp;
            }
            _background.color = _palette[_hp];
            return remainder;
        }

        public void UpdateLogic(float deltaTime)
        {
            
        }
    }
}