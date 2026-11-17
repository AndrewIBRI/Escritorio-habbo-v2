FROM php:8.2-apache

RUN apt-get update && apt-get install -y --no-install-recommends \
    netcat-openbsd \
    curl \
    && docker-php-ext-install pdo pdo_mysql mysqli \
    && a2enmod proxy proxy_http proxy_wstunnel rewrite headers \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /var/www/html

COPY docker/nitro/web/apache.conf /etc/apache2/sites-available/000-default.conf
COPY docker/nitro/web/configure_client.php /app/configure_client.php
COPY docker/nitro/web/portal/ /var/www/html/
COPY docker/nitro/web/client/ /var/www/html/hotel/
COPY docker/nitro/web/entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

EXPOSE 80 8064

ENTRYPOINT ["/entrypoint.sh"]
