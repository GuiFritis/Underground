using UnityEngine;
using UnityEngine.SceneManagement;

namespace Padrao.Utils
{
	public class LoadScene : MonoBehaviour
	{
		public void LoadSceneByName(string sceneName)
		{
			SceneManager.LoadScene(sceneName);
		}

		public void LoadSceneByIndex(int sceneIndex)
		{
			SceneManager.LoadScene(sceneIndex);
		}
	}
}