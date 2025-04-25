using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace GameUI
{
    [RequireComponent(typeof(Button))]
    public class StartButton : MonoBehaviour
    {
        private void Start()
        {
            var btn = GetComponent<Button>();
            btn.onClick.AddListener(TaskOnClick);
        }

        private void TaskOnClick()
        {
            SceneManager.LoadScene("Game");
        }
    }
}