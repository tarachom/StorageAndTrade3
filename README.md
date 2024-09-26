# "Зберігання та Торгівля" для України
<b>Облік складу, торгівлі та фінансів</b> | .net 8, Linux, Windows <br/>
    
Програма для ведення управлінського обліку і не містить бухгалтерських і податкових документів.
Товари на складах обліковуються по партіях, для номенклатури можна вести облік в розрізі характеристик та серійних номерів, для контрагентів облік взаєморозрахунків ведеться по договорах.

В програмі події фіксуються документами (Поступлення товарів, Замовлення, Реалізація і т.д).
Документи відображаються у журналах для кожного виду документу, документи можна вводити на основі інших документів, також можна подивитись рух документу по регістрах. 

    git clone https://github.com/tarachom/StorageAndTrade3.git
    git clone https://github.com/tarachom/Configurator3.git
    git clone https://github.com/tarachom/AccountingSoftwareLib.git   

<hr />
 
Детальніше про програму [accounting.org.ua](https://accounting.org.ua/storage_and_trade.html)<br/>
Середовище розробки [Visual Studio Code](https://code.visualstudio.com)<br/>
База даних [PostgreSQL](https://www.enterprisedb.com/downloads/postgres-postgresql-downloads)<br/>
Документація [GtkSharp](https://accounting.org.ua/watch/section/news/code-00000015) та [SQL](https://accounting.org.ua/watch/section/note/code-00000057)<br/>

<b>Для Ubuntu</b><br/>
Встановити dotnet-sdk-8.0

    sudo apt-get update
    sudo apt-get install -y dotnet-sdk-8.0

Довідка як встановити dotnet-sdk-8.0 на Ubuntu [Install .NET SDK or .NET Runtime on Ubuntu](https://learn.microsoft.com/uk-ua/dotnet/core/install/linux-ubuntu-install?tabs=dotnet8&pivots=os-linux-ubuntu-2204)

Встановити PostgreSQL

    sudo apt-get update
    sudo apt-get install postgresql

Встановити пароль для PostgreSQL

    sudo -u postgres psql
    \password postgres

Встановити Git

    sudo apt-get update
    sudo apt install git

<b>Для Windows</b> 
1. Встановити набір бібліотек GTK [gtk3-runtime](https://accounting.org.ua/download/gtk3-runtime-3.24.31-2022-01-04-ts-win64.exe)
2. Встановити dotnet 8.0 SDK [Download .NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
3. Встановити базу даних [PostgreSQL](https://www.enterprisedb.com/downloads/postgres-postgresql-downloads)
