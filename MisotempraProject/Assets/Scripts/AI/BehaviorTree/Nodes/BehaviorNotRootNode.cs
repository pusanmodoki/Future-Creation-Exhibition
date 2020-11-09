using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AI
{
	namespace BehaviorTree
	{
		namespace Node
		{
			namespace Detail
			{
				public abstract class NotRootNode : Detail.BaseNode
				{
					public ReadOnlyCollection<BaseService> services { get; private set; } = null;
					public ReadOnlyCollection<BaseDecorator> decorators { get; private set; } = null;
					protected List<BaseService> m_services { get; private set; } = null;
					protected List<BaseDecorator> m_decorators { get; private set; } = null;

					public bool isAllTrueDecorators
					{
						get
						{
							bool isResult = true;
							foreach (var e in decorators)
								isResult &= e.IsPredicate();
							return isResult;
						}
					}

					public NotRootNode() : base()
					{
						m_services = new List<BaseService>();
						m_decorators = new List<BaseDecorator>();
						services = new ReadOnlyCollection<BaseService>(m_services);
						decorators = new ReadOnlyCollection<BaseDecorator>(m_decorators);
					}

					protected void LoadDecoratorAndService(CashContainer.NotRootCashContainer container)
					{
						foreach(var serviceInfo in container.serviceClasses)
						{
							var type = System.Type.GetType(serviceInfo.className);
							m_services.Add((BaseService)JsonUtility.FromJson(serviceInfo.jsonData, type));
							m_services.Back().LoadBase(serviceInfo, type);
						}
						
						foreach (var decorator in container.decoratorClasses)
						{
							var type = System.Type.GetType(decorator.className);
							m_decorators.Add((BaseDecorator)JsonUtility.FromJson(decorator.jsonData, type));
							m_decorators.Back().LoadBase(decorator, type);
						}
					}

					protected void CloneDecoratorAndService(NotRootNode node)
					{
						foreach (var service in node.services)
						{
							m_services.Add((BaseService)JsonUtility.FromJson(
								BaseService.jsonData[service.guid], service.thisType));
							m_services.Back().CloneBase(service);
						}

						foreach (var decorator in node.decorators)
						{
							m_decorators.Add((BaseDecorator)JsonUtility.FromJson(
								BaseDecorator.jsonData[decorator.guid], decorator.thisType));
							m_decorators.Back().CloneBase(decorator);
						}
					}
				}
			}
		}
	}
}