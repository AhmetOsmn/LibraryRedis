# Redir Nedir?

- Açılımı *Remote Dictionart Service*'tir. Bazı kaynaklarda açılımı *Remote Directory Server* da olabilmektedir.

# Redis'in öncelikli kullanım amacı?
- Transact-SQL, MySQL gibi bir veritabanı teknolojisinin öncesinde bellek içi veritabanı olarak önbellekte kullanarak uygulamanın performansı arttırma amacıyla kullanılır.
 
- Redis'in desteklediği veri türleri şunlardır:

    - String
    - Bitmap
    - Hash
    - List
    - Set
    - Sorted Set
    - Geospatial Index
    - HyperLog
    - Stream

- Redis içerisinde verileri `:` ile gruplayabiliriz.

- Redis kullanılırken dikkat edilmesi gereken en önemli noktalardan birisi, Redis üzerinde bir sorun oluştuğu zaman projenin ayakta kalması, çalışmaya devam etmesidir. Redis çalışmıyorsa db'den veri getirerek veya db'ye veri götürerek programın çalışmaya devam etmesi sağlanmalıdır.

- Redis normalde asenkron olarak çalışmaz. Gönderilen işlemleri sırayla tamamlayarak devam eder. Eğer Redis içerisindeki *Pipelined* özelliği aktif edilip kullanılırsa Redis'in işemleri tamamlama hızı ortalama 3 kat artar. Pipelined özelliği ise şunu yapar: Gönderilen işlemleri toplar ve  hepsini bir anda yapar.

- Eğer Redis üzerinde *Durability* aktif edilmemiş ise, sunucu kapandığı anda veriler silinir. *Durability* aktif edilmiş ise sunucu kapandığında RAM içerisindeki veriler disk'e yazılır.

- Programımıza veri geldiğinde, silindiğinde veya güncellendiğinde Redis içerisindeki verileri güncellemeyi unutmamalıyız. Örnek olarak daha öncesinde güncellenen bir veriyi redis içerisinde güncellememişsek, sonraki sorgularda veriyi redis üzerinden getirdiğimiz zaman yanlış veri getirmiş olacağız.

- Temel olarak `Key:Value` olacak şekilde hazırlanan bir `NoSQL` veri tabanıdır diyebiliriz.

- NoSQL Veri Tabanı: 

- In Memory Database yaklaşımını benimseyerek çalışır, eğer tercih edersek opsiyonel olarak disk üzerine yazılabilme özelliği de vardır.

- Oluşturulan instance'lardan bir tane **Master** olarak ayarlanır. Geriye kalan diğer instance'lar **Slave** olarak kalır ve bunlar *Master*'in kopyalarıdır.

- Slave instance'ler default olarak sadece okuma işlemi yaparlar. Eğer yazma işlemi yapılacak ise Master instance kullanılır.

- Redis'i kullanıcak olan client bu instance'lardan (slave ya da master farketmez) birisine bağlanır.

<br>

# Redis Ne İçin Kullanılır?

- Redis'in amacı db'den yükü alıp client'a verileri hızlı vermektir.

- Redis'in en çok kullanıldığı alanlardan birisi `Caching` işlemleridir. Bunun önemli bir nedeni Redis'in bize sunmuş olduğu hızdır.

- Eğer istenilirse *RabbitMQ* gibi veya *Apachi Kafka* gibi `Messsage Broker` olarak kullanılabilir.

- Redis sakladığı verileri disk üzerine yazabildiği için db olarak kullanılabilir fakat asıl amacı bu değildir.

- Oturum kontrollerinde kullanılabilir.

- Filtreleme işlemleri yapan bir servis içerisinde kullanılabilir.

<br>

# Redis'in avantajları nelerdir?

- Redis bilinçli olarak dayanıklılıktan feragat ederek hızı arttırmaya odaklanır.

- Bir sistem hatası durumunda Redis diske veri yazar ancak geri kalabilir, bu durum kaydedilmemiş verinin kaybına sebep olabilir.

- Redis'i öne çıkartan özelliği hızlı çalışmasıdır.

- Eğer veriler çok sık değişmiyor ancak sürekli talep ediliyorsa kullanılmalıdır.

- Veri akışının istenmeyen bir şekilde kesilme durumlarında devreye giren sistemleri vardır. Bunlar:

    - Manuel 
    - Redis Sentinel
    - Redis Cluster

- RDB (Redis Database Snapshots): Db'nin direkt olarak kopyası alabiliriz.


<br>

# Redis'in dezavantajları nelerdir?

- Anlık verilerimiz çok fazla değişiyorsa Redis ile cache işlemi tercih edilmeyebilir. Bunun yerine daha stabil veri setleri için kullanılır.
- Tek thread çalışır.

<br>

# Redis'i Kullanmak

1. Redis'i *Docker* ile de kullanabiliriz, sonraki başlığın içeriği o konu hakkında.

    İlk olarak *Windows* içerisinde *Ubuntu* ile işlemler yapabilmek için bir düzenleme yapmamız gerekecek. Alt kısımda gösterilen alandan *Winodws Subsystem for Linux* seçeneğini açalım.

    | Turn Windows Features | Subsystem |
    |-|-|
    |![turn](./UI/wwwroot/image/readme/01-turnwindows.png)| ![subsystem](./Images/02-windowssubsystem.png)|
    
2. Microsoft Store'dan *Ubuntu* uygulamasını indirelim.

    ![Ubuntu](./Images/03-ubuntu.png)

    *Ubuntu*'yu indirdikten sonra uygulamayı açtığımızda karşımıza bir CLI açılacak.
    
     Karşımıza çıkan ekranda biraz beklememiz istenecek ve daha sonrasında *Ubuntu*'yu kullanırken giriş yapacağımız bir hesap için *user name* ve *password* alanlarını girmemiz istenecek.

     Bilmeyen arkadaşlar olursa diye belirtmek istiyorum, bu alanda gireceğiniz şifre CLI ekranında görünmeyecektir. Şifrenizi girdikten sonra `Enter` ile ilerlediğinizde şifrenizi onaylatıp sonraki aşamalara geçebilirsiniz.

     Son olarak artık *Ubuntu* içerisinde işlem yapabilir hale geleceğiz. Örnek olarak olarak alt kısımdaki gibi bir CLI ekranı görüyor olmalıyız.

     ![ubuntucli](./Images/04-ubuntucli.png) 

3. *Ubuntu* içerisinden, Redis'i kullanabilmek için bazı paketler yüklememiz gerekecek. İlk olarak *Redis Labs* içerisinden *redis* repository'sini indirelim.

    ![addrepo](./Images/05-addrepository.png)

4. Repo yüklendikten sonra *Ubuntu* içerisinde *Update* ve *Upgrade* komutları ile versiyonların güncelliğini kontrol edelim.

    ![updateandupgrade](./Images/06-ubuntuupdateandupgrade.png)

5. Güncelleme işlemlerinden sonra artık *Redis Server* paketini ekleyebiliriz.

    ![addredisserver](./Images/07-installredisserver.png)

    İndirme işleminden sonra alt kısımdaki gibi *Redis Server* için versiyon kontrolü yaparak bir sorun olmadığını kontrol edebiliriz.

    ![redisserverversion](./Images/08-redisserverversion.png)

6. Bu aşamaya kadar bir sorun ile karşılaşmadıysak alt kısımda olduğu gibi *Redis Server*'ı aktif hale getirip kullanabiliriz.

    ![runredisserver](./Images/09-runredisserver.png)

    Yukarıdaki ekran görüntüsünün alt kısmında yeşil olarak gösterilen alanda server'ın bağlantılar için hazır olduğu bilgisini görebiliriz. 

7. *Redis Server* çalışırken yeni bir Ubuntu CLI açıp server ile iletişim kuralım.

    ![userediscli](./Images/10-userediscliwithserver.png)

    Server aktif olduğu için Redis CLI'a erişim sağlayabildik. Artık Redis'e ait olan komutlar ile istediğimiz işlemleri buradan gerçekleştirebiliriz. 

    Peki *Redis Server*'ı kapatmak istersek veya server'ın kapalı olduğunu anlamak istersek ne yapabiliriz? 

    İlk olarak server'ı kapatmak istersek bunun farklı yolları var. Alt kısımda verilen örnekte bütün server'ları kapatmak istediğimizi kırmızı olarak işaretlenen komut ile söylüyoruz.

    ![connecterror](./Images/11-connecterror.png)

    Server'ları kapatma komutu ile arka planda *Redis Server*'ın durdurulduğunu sol altta kırmızı olarak işaretlenen alandan görebiliriz. Server'ın çalıştığı *Ubuntu CLI* artık bizden komut alabilir hale geldi.

    Biz server kapalı haldeyken Redis CLI'a bağlanmak istersek sağ taraftaki Ubuntu CLI'da gördüğümüz gibi bize server'a bağlanamadığımız ile ilgili bir hata gönderecektir.

    Server'ın aktif olup olmadığını öğrenmenin başka bir yolu da alt kısımdaki kırmızı olarak işaretlenen komuttur.

    ![ping](./Images/12-ping2.png) 

    Eğer server aktif ise `ping` komutumuzda bize `PONG` yanıtı gelecektir. Eğer serfer aktif değil ise mavi olarak işaretlenen kısımlarda görüldüğü gibi bağlantı olmadığına dair yanıt dönecektir.

<br>

# Docker İle Redis Kullanmak



<br>

# Çok Kullanılan Redis CLI Komutları

- `KEYS *`: Var olan Key'leri listeler.

- `SET name Ahmet`: name isimli bir key oluşturur ve değer olarak Ahmet atar. 
- `GET name`: name isimli key'in içerisindeki değeri getirir.
- `EXISTS name`: name isimli key mevcut ise 1 (int), mevcut değil ise 0 (int) döndürür. 
- `DEL name`: name isimli key'i siler ise 1 (int), silemezse (örnek olarak name key'i mevcut değilse) 0 (int) döndürür.
- `APPEND name Ahmet`: name isimli key mevcut değilse içerisinde Ahmet verisi olan name isimli bir key oluşturur. Eğer name isimli key mevcut ise, key'in içerisindeki değerin sonuna Ahmet değerini ekleyerek birleştirir. Geriye oluşan yeni değerin karakter uzunluğunu döndürür.
- `HSET kedi cins tekir`: kedi isimli bir nesne üretmemizi, kedi nesnesinin içerisinde cins property'si üretmemizi ve property'e tekir değerini atamamızı sağlar. Bu şekilde nesneler tanımlayıp tutabiliriz.
- `HGET kedi cins`: Oluşturulan kedi nesnesinin cins property'sinde tutulan değeri getirir.
- `HGETALL kedi`: Oluşturulan kedi nesnesinin bütün property'lerini property ismi ve içerisindeki değer şeklinde sıralayarak getirir.
- `SET geciciKediIsmi maya EX 5`: 5 saniye sonra otomatik olarak yok olacak bir geciciKediIsmi key'i oluşturur. 5 saniye sonra key otomatik olarak silinir.
- `PUBLISH HufflePuff "redis ogreniyoruz"`: HufflePuff kanalının üyelerine "redis ogreniyoruz" mesajı gönderir.
- `SUBSCRIBE HufflePuff`: HufflePuff kanalına abone oluruz. Kanal içerisinde mesaj yayınlandığında aboneler tarafından mesaj görüntülenir.

<br>

# Redis ile örnek proje:





<br>

# Kaynak

- (Kablosuz Kedi)[https://www.youtube.com/watch?v=JLS9gg-oJPQ]
- (Bora Kaşmer)[https://www.youtube.com/c/borakasmer]
- (Redis Kurmak)[https://www.youtube.com/watch?v=_nFwPTHOMIY]
- (Redis)[https://redis.io/]
