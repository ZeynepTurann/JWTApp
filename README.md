# JWTApp
﻿Burada hedeflenen ana amaç, bir .NET 6 projesi geliştirerek, UI tarafıyla haberleşmektedir.

Öncelikli olarak, Bu projemizde Onion Architecture uygulayarak bir mimari pattern oluşturmaya çalışcağız. Oluşturduğumuz patterne alışık olduğumuz Generic Resository Pattern entegrasyonu gerçekleştireceğiz. ApiController üzerinde gelen istekleri yönlendirecek, yönlenen route üzerinden, request karşılayıp daha sonrasında bu istekleri tekrardan controllerlar ile haberleştirebilecek olan mediator pattern entegrasyonu gerçekleştirilecektir.

Mediator pattern entegrasyonu gerçekleştirilirken, NuGeT Packages'dan MediatR kütüphanesiden besleneceğiz.

Bu kurulumlar tamamlandıktan sonra, iş kullanıcı tarafına geldiğinde, yükümüzü microsoft identity ile birlikte biraz hafifleteceğiz. Identity kullanımı sırasında JWT(Bearer) dahil edilerek, yönetilen isteklere token bilgisi cookie'ler aracalığıyla isteklerinde tokenlar ile birlikte hareket etmesini sağlayacağız.

Onion Architecture için Core ve Persistance klasörleri oluşturuldu.

Core sistem içeirinde her katmana referans olmak istediğim çekirdek katmanımdır, ayrıca bu katmanım hiyerarşi ilişkini en ortasında kaldığı için UI/Presentation katmanlarından ayrıştırmış olucam, bude benim için basit bir güvenlik açığı vermeme konusunda bana yardımcı olacak.

Core içeriisnde Application ve Domain klasörlerini oluşturuyorum, Domain burada entityleri tutacaktır, Application içerisindekilerle haberleşecektir.

Persistance katmanı/klasörü içerisine, FluentApi'ler için bir configurations, Context için Context, Migration ve Repositories klasörleri oluşturulur.

Configurations ayarlamaları AppUser ve Product için tamamlanır. Yapılan konfigürasyonlar uygulanabilmesi adına Context oluşturulur.

context tamamlandıktan sonra persistance > Migrations klasörü içerisine migration çıkmak istiyorum

add-migration InitialCreate -OutputDir Persistance/Migrations => ilgili -OutputDir keyword'ü kaynak hedef göstererek migrations dosyalarını tutacaktır

Daha sonrasında, Apllication > Interfaces klasörü açılır ve generic repository pattern için interface oluşturulur.

Oluşturulan bu interface, Persistance > Repositories içerisinde implement edilir.

Implementasyon tamamlandıktan sonra, IoC container'ına dependency injection eklenir.

Mediator pattern uygulama aşamasına geçiyoruz.

Mediator pattern, bir nesnenin diğer nesnelerle etkileşimlerini yöneten bir nesne olarak tanımlanır. Bu yapı, nesneler arasındaki ilişkileri düzenler ve bu nesnelerin birbirleriyle direkt olarak etkileşimde bulunmamasını sağlar. Bu sayede nesneler arasındaki ilişkiler daha anlaşılır hale gelir ve değiştirilebilir hale gelebilir.

Mediator pattern'ın avantajları, sistemdeki nesneler arasındaki ilişkilerin düzenlenmesi ve değiştirilebilirliği, aynı zamanda da sistemdeki nesnelerin birbirleriyle direkt olarak etkileşimde bulunmamasıdır. Bu sayede nesneler arasındaki ilişkiler daha anlaşılır hale gelir ve sistem daha esnek hale gelebilir.

Mediator pattern'ın bir örnek olarak, bir havaalanında uçakların iniş yapıp kalkmasını düzenleyen bir kontrol kulesi gösterilebilir. Uçaklar kontrol kulesinin yönetimi altında iniş yapar ve kalkarlar, ancak uçaklar birbirleriyle doğrudan iletişim kurmazlar. Kontrol kulesi, uçakların havaalanında ne zaman iniş yapacağını ve ne zaman kalkacağını düzenler ve bu sayede uçaklar arasındaki ilişkiler düzenlenmiş olur.

Bir başka örnek olarak, bir sohbet uygulamasında kullanıcıların birbirleriyle iletişim kurmasını düzenleyen bir server gösterilebilir. Kullanıcılar server'a bağlı olurlar ve server aracılığıyla birbirleriyle iletişim kururlar. Bu sayede kullanıcılar birbirleriyle doğrudan iletişim kurmazlar ve server aracılığıyla iletişim kurarlar. Bu örnekte, server mediator pattern'ın bir örneğidir ve kullanıcılar arasındaki iletişimi düzenler.

Başka bir örnek olarak, bir otomatik makine sisteminde makine ve parçalar arasındaki iletişimi yöneten bir kontrol paneli gösterilebilir. Makine ve parçalar birbirleriyle doğrudan iletişim kurmazlar ve kontrol paneli aracılığıyla iletişim kurarlar. Kontrol paneli, makine ve parçalar arasındaki iletişimi düzenler ve bu sayede sistem daha anlaşılır ve değiştirilebilir hale gelir.

Bu patterini uygulamak adına bir kütüphane indirebilirsiniz.

MediatR.Extensions.Microsoft.DependencyInjectinon (latest version) indirilir.

Bu paket içerinde MediatR kütüphanesini de barındırdığı için tekrar onu da indirmemize gerek kalmaz.

Mediator Pattern ilişkilerini temsil edebilmek dosya altyapılarını hazırlıyorum.

Core > Features > CQRS > Commands > Handlers > Queries klasörleri oluşturulur.

Core > DTOs klasörü oluşturulur.

ProductDto oluşturulur.

Burada ihtiyacımız olacak queryler için, az önce oluşturduğumuz Queries dosyası içeriisnde querylerimizi tutacak sınıflarımızı oluşturuyoruz.

Query Sınıfı oluşturulur.

Daha sonrasında Handler klasörü içeriisne, Bu query yakalayacak ve gerekli işlemleri yapıcak handler sınıfı oluşturulur.

Tam burada auto mapper dependency injection kütüphanesi indirilip, düzenleme işlemlerine başlanır.

Application > Mappings Klasörü oluşturulur ve içerisine,

ProductProfile class'ı oluşturulur.

Oluşturulan bu class'ın di işlemleri için program.cs'e kayıt edilir.

Handler içerine IMapper di tamamlandıktan sonra, Handle metodunun dönüşünde mapperımı kullanıyorum.

Yazmış olduğumuz GetAllProduct requestini çağıracak olan controller yapısını oluşturalım.

Action ayarlamaları gerçekleşirildikten sonra, diğer request ve handlerlar oluşturularak, diğer actionlarımıza bağlanır.

Bu işlemler tamamlandıktan sonra, aynı gibi gözüken biraz daha farklı varyasyonlarını categories controller oluşturarak, endpointlerine yeni isteklerimi bağladım.

CRUD operasyonları tamamlandıktan sonra, artık kullanıcı tarafını ele alabiliriz.

Öncelikle kullanıcı kayıt işlemleri(register) tamamlanır.

Daha sonrasında login endpointlerinde tüketilmek üzere request ve dtolar tanımlanır.

İşte bu senaryoda kullanmak istediğimiz Authenticatin Schemelarını dahil edebiliriz.

Biz burada JwtBearer token kullanacağız.

Bu token header içerisine gömüp öyle taşıyacağız.(ki bu en yüksek düzey güvenliklerden biridir).

UI tarafından, API Client'a istek olarak gönderildiğinde ise, Token'ın headerda olduğu bilgini cookie'ye atarak ileteceğiz.

JWTSettings, token ayarlamalarını düzenlemek için açtığımız sınıftır.

JWTResponse, JWTGenerator'ın doldurup sistem içerisine ileteceği Token Modelini tutan class.

JWTGenarator'ın hedefi ise, Middleware'lar arasına kayıt edilen token algoritmasına uyacak şekilde token üretmek olan bir class'tır.

Artık istenilen rol mekanızmasi ve kimlik doğrulamalar sisteme dahil edilmiştir, UI tarafına geçilir..

---------------------------------------------- UI -----------------------------------------------

ASP.Net Core Empty Project oluşturulur.

Program.cs düzenlenir.
