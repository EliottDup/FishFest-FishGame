using UnityEngine;

public class Diver : MonoBehaviour
{
    enum DiverState
    {
        Patrol,
        Alarmed,
        Hunting,
        Working
    }

    #region UI
    float areaSize = 100;

    [Header("Patrol")]

    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float timeBeforeNewTarget;
    [SerializeField] float targetRange;
    [SerializeField] float shipRadius;

    #endregion UI

    DiverState state = DiverState.Patrol;
    Vector3 target;

    [SerializeField] LayerMask _groundLayer;

    void Start()
    {
        ChooseNewTarget();
    }

    void Update()
    {
        if (state != DiverState.Working)
        {
            if (MoveTowardsTarget())
            {
                ChooseNewTarget();
            }
        }
    }

    void ChooseNewTarget()
    {
        bool searchingPos = true;
        int i = 0;
        target = Vector3.zero;

        while (searchingPos && i < 10)
        {

            float x = Random.Range(Mathf.Clamp(transform.position.x + targetRange, -areaSize / 2, areaSize / 2), Mathf.Clamp(transform.position.x - targetRange, -areaSize / 2, areaSize / 2));
            float z = Random.Range(Mathf.Clamp(transform.position.z + targetRange, -areaSize / 2, areaSize / 2), Mathf.Clamp(transform.position.z - targetRange, -areaSize / 2, areaSize / 2));

            target = new Vector3(x, 0, z);
            print(target);
            if (target.magnitude > shipRadius)
            {
                searchingPos = false;
            }
            i++;
        }

    }

    bool MoveTowardsTarget()
    {

        Vector3 lookTarget = new Vector3(target.x, transform.position.y, target.z);

        Vector3 targetDir = (lookTarget - transform.position).normalized;

        Vector3 lookDir = Vector3.RotateTowards(transform.forward, targetDir, rotationSpeed * Time.deltaTime, 0.0f);

        transform.rotation = Quaternion.LookRotation(lookDir);

        Vector3 curpos = transform.position;
        // curpos = Vector3.MoveTowards(curpos, target, moveSpeed * Time.deltaTime);

        RaycastHit hit;
        print((lookDir.normalized - targetDir.normalized).magnitude);
        if (Physics.Raycast(curpos + Vector3.up * 10, Vector3.down, out hit, 30, _groundLayer) && (lookDir.normalized - targetDir.normalized).magnitude < 0.5f)
        {
            curpos += transform.forward * moveSpeed * Time.deltaTime;
            transform.position = new Vector3(curpos.x, hit.point.y + 1, curpos.z);
        }

        if (FlatDist(transform.position, target) < moveSpeed * Time.deltaTime)
        {
            return true;
        }
        return false;
    }

    float FlatDist(Vector3 v1, Vector3 v2)
    {
        Vector2 p1 = new Vector2(v1.x, v1.z);
        Vector2 p2 = new Vector2(v2.x, v2.z);
        return Vector2.Distance(p1, p2);
    }
}
