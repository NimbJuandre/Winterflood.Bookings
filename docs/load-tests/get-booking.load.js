import http from 'k6/http';
import { check, sleep } from 'k6';
import { Trend } from 'k6/metrics';

const BASE_URL = __ENV.BASE_URL || 'http://localhost:5249';

const getDuration = new Trend('get_booking_duration', true);

export const options = {
  scenarios: {
    // Ramp virtual users up, hold, then ramp down (a classic stress profile).
    ramping_load: {
      executor: 'ramping-vus',
      startVUs: 0,
      stages: [
        { duration: '15s', target: 50 },   // ramp up to 50 VUs
        { duration: '15s', target: 50 },    // hold at 50 VUs
        { duration: '15s', target: 100 },   // spike to 100 VUs
        { duration: '15s', target: 0 },     // ramp down
      ],
      gracefulRampDown: '10s',
    },
  },
  thresholds: {
    // 95% of requests must complete under 200ms, 99% under 500ms.
    http_req_duration: ['p(95)<200', 'p(99)<500'],
    // Less than 1% of requests may fail.
    http_req_failed: ['rate<0.01'],
    get_booking_duration: ['p(95)<200'],
  },
};

export function setup() {
  const payload = JSON.stringify({
    customerName: 'Load Test',
    bookingType: 'Apartment',
    itemName: 'Seaside flat',
    startDate: '2026-06-01',
    endDate: '2026-06-07',
    quantity: 1,
  });

  const res = http.post(`${BASE_URL}/api/bookings`, payload, {
    headers: { 'Content-Type': 'application/json' },
  });

  check(res, {
    'setup: booking created (201)': (r) => r.status === 201,
  });

  return { bookingId: res.json('id') };
}

export default function (data) {
  const res = http.get(`${BASE_URL}/api/bookings/${data.bookingId}`, {
    tags: { name: 'GetBookingById' },
  });

  getDuration.add(res.timings.duration);

  check(res, {
    'status is 200': (r) => r.status === 200,
    'body has matching id': (r) => r.json('id') === data.bookingId,
  });

  sleep(1);
}
