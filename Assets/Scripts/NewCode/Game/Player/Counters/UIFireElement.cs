using UnityEngine;
using UnityEngine.UI;

namespace Garden
{
    public class UIFireElement : MonoBehaviour
    {
        [SerializeField] private Image _background;
        private FireData _fireData;
        private Color[] _palette;
        
        public void Init(FireData fireData, Color[] palette)
        {
            _fireData = fireData;
            _palette = palette;
        }

        public void UpdateView()
        {
            _background.color = Color.Lerp(_palette[0], _palette[1], _fireData.Time / _fireData.StartTime);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}