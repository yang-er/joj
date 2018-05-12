#include<iostream>
#include<stdlib.h>
#include<queue>
#include<fstream>

using namespace std;


bool compare(int a, int b)
{
	return a < b; //升序排列，如果改为return a>b，则为降序
}

int main() {
	int a;              //整数n
	int j;
	cin >> a;
	int b = a;
	int s[30001];       //珍珠重量
	int h[30000];
	for (int i = 0; i < a; i++) {
		cin >> s[i];
	}
	for (int i = 0; i < b - 1; i++) {
		sort(s, s + a, compare);
		h[i] = s[0] + s[1];
		a = a - 1;
		for (j = 2; j < b; j++) {
			s[j - 2] = s[j];
		}
		s[j - 2] = h[i];
	}
	int sum=0;
	for (int i = 0; i < b - 1; i++) {
		sum += h[i];
	}
	cout << sum;
	return 0;
}