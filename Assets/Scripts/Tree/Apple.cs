using UnityEngine;
using System.Collections;
using BASE;

namespace INTERACT {
    public class Apple : MonoBehaviour {
        public float expandSpeed;
        public float maxSize;

        private float speed;
        private void Start() {
            transform.localScale = Vector3.zero;
        }

        private void Update() {
            if (transform.localScale.x < maxSize) {
                transform.localScale += expandSpeed * Time.deltaTime * Vector3.one;
            }
        }

        public void Drop() {
            StartCoroutine(DropCoroutine());
        }

        private IEnumerator DropCoroutine() {
            while (transform.position.y > GameModel.Instance.heightOffset.y) {
                speed += 0.1f * Time.deltaTime;
                transform.position += speed * Vector3.down;
                yield return null;
            }
        }
    }
}
