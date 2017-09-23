using UnityEngine;

public class test : MonoBehaviour {

    public void Show() {
        transform.position = new Vector3(0, GameModel.Instance.heightOffset, 2);
    }

}
