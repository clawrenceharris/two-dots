
using System.Collections;

public interface IHittableVisualController{
    IEnumerator Hit();
    IEnumerator Clear(float duration);
}