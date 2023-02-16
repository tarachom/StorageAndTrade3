# "Зберігання та Торгівля" для України
 <img src="https://accounting.org.ua/images/storage_and_trade.ico?v=3" /> <b>Програма для обліку торгівлі, сладу та фінансів </b> | .net 7, Linux, Windows <br/>
   
 <b>Можливості</b>
    
    Облік по багатьох організаціях
    Облік товарів по партіях
    Облік номенклатури по характеристиках
    Облік по серійних номерах
    Облік взаєморозрахунків з контрагентами по договорах

    Події фіксуються документами (Рахунок фактура, Замовлення клієнта і т.д)
    Документи відображаються у журналах для кожного виду документу
    Додатково є збірні журнали (Продажі, Закупки, Фінанси, Склад і т.д)
    
    Документи можна вводити на основі інших документів
    Також можна подивитись рух документу по регістрах
    
    Є можливість вигрузки/загрузки даних та обмін даними між різними базами.
    
 Облік торгівлі (документи)

    Рахунок фактура
    Замовлення клієнта
    Реалізація товарів та послуг
    Акт виконаних робіт
    Замовлення постачальнику
    Поступлення товарів та послуг
    Повернення клієнту
    Повернення постачальнику

Облік складу (документи)

    Переміщення товарів між складами
    Встановлення цін номенклатури
    Введення залишків
    Внутрішнє споживання

Облік фінансів (документи)

    Прихідний касовий ордер
    Розхідний касовий ордер

Звіти

    Замовлення клієнтів
    Замовлення постачальникам
    Партії товарів
    Вільні залишки
    Товари на складах
    Рух коштів
    Розрахунки з контрагентами
    Закупівлі
    Продажі

<hr />
<br/>

 <b>Встановлення dotnet-sdk для Ubuntu 22.10</b>
 
 Детальніше - [Install the .NET SDK or the .NET Runtime on Ubuntu](https://learn.microsoft.com/uk-ua/dotnet/core/install/linux-ubuntu)<br/>
 
    wget https://packages.microsoft.com/config/ubuntu/22.10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
    sudo dpkg -i packages-microsoft-prod.deb
    rm packages-microsoft-prod.deb
    
    sudo apt-get update && sudo apt-get install -y dotnet-sdk-7.0
    
    # Переглянути детальну інформацію про встановлені версії sdk і runtimes
    dotnet --list-sdks && dotnet --list-runtimes

<br/>

 <b>Встановлення PostgreSQL для Ubuntu</b>
 
 Детальніше - [PostgreSQL](https://www.postgresql.org/download/linux/ubuntu/)<br/>
 
    sudo sh -c 'echo "deb http://apt.postgresql.org/pub/repos/apt $(lsb_release -cs)-pgdg main" > /etc/apt/sources.list.d/pgdg.list'
    wget --quiet -O - https://www.postgresql.org/media/keys/ACCC4CF8.asc | sudo apt-key add -
    
    sudo apt-get update
    
    sudo apt-get -y install postgresql

    # Встановлення пароля для postgres
    sudo -u postgres psql
    \password postgres
    
    # Переглянути детальну інформацію про встановлену програму postgresql
    dpkg -l | grep postgresql

<br/>

 <b>Встановлення Git</b>
    
    sudo apt install git

<br/>

 <b>Збірка програми</b>
    
    git clone https://github.com/tarachom/StorageAndTrade3.git
    git clone https://github.com/tarachom/AccountingSoftwareLib.git
    
    dotnet build StorageAndTrade3 --output StorageAndTrade3/bin/Debug/net7.0
    
    mkdir -p bin
    cp -r StorageAndTrade3/bin/Debug/net7.0/* bin

<hr />
 
  Детальніше про програму [accounting.org.ua](https://accounting.org.ua/storage_and_trade.html)<br/>
  Середовище розробки [Visual Studio Code](https://code.visualstudio.com)<br/>
  База даних [PostgreSQL](https://www.enterprisedb.com/downloads/postgres-postgresql-downloads)<br/>
