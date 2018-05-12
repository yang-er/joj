#include<stdio.h>
#include<iostream>
#include<list>
using namespace std;
struct L {
	long long ii;
	bool operator< (L& ano) {//要重载一个小于运算符才能排序，先按i排序，如果想等在按c排序
		return ii < ano.ii;//return < 是按升序排序，想要是降序要换成>
	}
};
int main()
{
	list<L> li;
	L date[30005];
	long long  n = 0,j=0;
	cin >> n;
	if(n==0)cout<<0<<endl;
	else
    {
	for (int i = 0; i < n; i++)
	{
		cin >> j;
		date[i].ii = j;
		li.push_back(date[i]);
	}
	li.sort();
	long long zui = 0;
	for (int i = 0; i < n - 1; i++)
	{
		int zhong = 0;
		L aa = li.front();
		li.pop_front();
		zhong += aa.ii;
		aa = li.front();
		li.pop_front();
		zhong += aa.ii;
		aa.ii = zhong;
		li.push_back(aa);
		li.sort();
		zui += zhong;
	}
	cout << zui<< endl;
    }
	fclose(stdin);
	fclose(stdout);
}
