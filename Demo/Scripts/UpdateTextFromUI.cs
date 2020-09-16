using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MazeGeneration.Demo
{
    [RequireComponent(typeof(TMP_Text))]
    public class UpdateTextFromUI : MonoBehaviour
    {
        #region Variables
        [SerializeField] private TMP_Text m_text;
        #endregion

        #region Mono
        protected void Reset()
        {
            m_text = GetComponent<TMP_Text>();
        }
        #endregion

        #region Mutators
        public void SetText(int value)
        {
            m_text?.SetText($"{value}");
        }

        public void SetText(float value)
        {
            m_text?.SetText($"{value}");
        }
        #endregion
    }
}