using UnityEngine;

public class Menu : MonoBehaviour
{
    public string menuName;
    public bool isOpened;
    public CursorLockMode cursorLockMode;

    public void Open()
    {
        gameObject.SetActive(true);
        Cursor.lockState = cursorLockMode;
        isOpened = true;
    }

    public void Close()
    {

        gameObject.SetActive(false);
        isOpened = false;

    }
}
