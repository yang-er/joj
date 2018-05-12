#include <iostream>

#include <queue>

#include <stdio.h>

using namespace std;
int main()
{
	vector<int> v;
	int all=0;
	int sum;
	int m1,m2;
	cin>>sum;
	for(int i=0;i<sum;i++)
	{
		int temp;
		scanf("%d",&temp);
		v.push_back(temp);
	}
	std::vector<int>::iterator it;
	while(v.size()!=1)
	{
	for(it=v.begin(),m1=*it;it!=v.end();it++)
	{
		if(m1>*it)
		{
			m1=*it;
		}
	}
	for(it=v.begin(),m1=*it;it!=v.end();it++)
	{
		if(m1==*it)
		{
			it=v.erase(it);
			break;
		}
	}
	for(it=v.begin(),m2=*it;it!=v.end();it++)
	{
		if(m2>*it)
		{
			m2=*it;
		}
	}
	for(it=v.begin(),m2=*it;it!=v.end();it++)
	{
		if(m2==*it)
		{
			it=v.erase(it);
			break;
		}
	}
	all+=(m1+m2);
	v.push_back(m1+m2);
	}
	cout<<all;
	fclose(stdin);
	fclose(stdout);
	return 0;
}

