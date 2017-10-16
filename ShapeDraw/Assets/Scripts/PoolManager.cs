using UnityEngine;

namespace ShapeDraw
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        [SerializeField]
        private Transform _container;

        [Header("Prototypes:")]
        public GameObject pointPrototype;
        public GameObject linePrototype;
       
        public UnityPool<ShapePart> PointsPool { get; protected set; }
        public UnityPool<ShapePart> LinesPool { get; protected set; }

        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        protected override void Init()
        {
            base.Init();

            if (_container == null)
            {
                var obj = new GameObject("Pools");
                _container = obj.transform;
                _container.SetParent(transform, false);
            }

            InitPools();

            Precache();
        }

        protected void InitPools()
        {
            PointsPool = new UnityPool<ShapePart>(() => Instantiate(pointPrototype).GetComponent<ShapePart>(), _container, "Point");
            LinesPool = new UnityPool<ShapePart>(() => Instantiate(linePrototype).GetComponent<ShapePart>(), _container, "Line"); 
        }

        protected void Precache()
        {
            PointsPool.Cache(3);
            LinesPool.Cache(3);
        }
    }
}