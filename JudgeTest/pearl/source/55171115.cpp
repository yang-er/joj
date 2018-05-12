#include <iostream>
#include <algorithm>
#include <vector>
#include <fstream>
#include<queue>
using namespace std;

int _count=0;
int main()
{
   int n;
   cin>>n;
   priority_queue<int,vector<int>,greater<int>> i;
   for(int j=0;j<n;j++)
   {
       int m;
       cin>>m;
       i.push(m);
   }
   while(i.size()!=1)
   {
       int m=i.top();
       i.pop();
       int n=i.top();
       i.pop();
       i.push(m+n);
       _count=_count+m+n;
   }
   //_count=_count+_v[0];
   cout<<_count;
    return 0;
}
