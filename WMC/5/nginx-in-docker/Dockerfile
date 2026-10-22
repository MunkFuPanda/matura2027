FROM nginx:stable-alpine
COPY test-proj/dist/test-proj/browser /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]