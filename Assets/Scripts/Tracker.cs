using UnityEngine;

public class Tracker : MonoBehaviour
{
    void Update()
    {
        // Verificar si se hace clic con el botón izquierdo del ratón
        if (Input.GetMouseButtonDown(0))
        {
            // Lanzar un rayo desde la posición del ratón
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Comprobar si el rayo interseca con algún objeto
            if (Physics.Raycast(ray, out hit))
            {
                // Obtener el nombre del objeto golpeado
                string objectName = hit.collider.gameObject.name;

                // Imprimir el nombre del objeto en la consola
                Debug.Log("Objeto apuntado: " + objectName);
            }
        }
    }
}
