//-----------------------------------------------------------------------
// <copyright file="ByteBuffer.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2016 Typesafe Inc. <http://www.typesafe.com>
//     Copyright (C) 2013-2016 Akka.NET project <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace Akka.IO
{
    public class ByteBuffer
    {
        private byte[] _array;
        private int _offset;
        private int _capacity;

        private int _position;
        private int _limit;

        private ByteOrder _order;



        public ByteBuffer(byte[] array)
            : this(array, 0, array.Length)
        {
        }

        public ByteBuffer(byte[] array, int offset, int length)
        {
            _array = array;
            _offset = offset;
            _capacity = length;

            _position = 0;
            _limit = length;
        }

        public void Clear()
        {
            _position = 0;
            _limit = _capacity;
        }

        public void Position(int value)
        {
            if ((value > _limit) || (value < 0))
                throw new ArgumentException();
            _position = value;
        }
        public void Limit(int value)
        {
            if ((value > _capacity) || (value < 0))
                throw new ArgumentException();
            _limit = value;
            if (_position > _limit) _position = _limit;
        }

        public void Flip()
        {
            _limit = _position;
            _position = 0;
        }

        public bool HasRemaining
        {
            get { return _position < _limit; }
        }

        public int Remaining
        {
            get { return _limit - _position; }
        }

        public byte[] Array()
        {
            return _array;
        }

        public void Put(byte[] src)
        {
            var len = Math.Min(src.Length, Remaining);
            System.Array.Copy(src, 0, _array, _offset + _position, len);
            _position += len;
        }

        public static ByteBuffer Wrap(byte[] array, int start, int len)
        {
            return new ByteBuffer(array, start, len);
        }
        public static ByteBuffer Wrap(byte[] array)
        {
            return new ByteBuffer(array, 0, array.Length);
        }

        public static ByteBuffer Allocate(int capacity)
        {
            return new ByteBuffer(new byte[capacity]);
        }

        public void Order(ByteOrder byteOrder)
        {
            _order = byteOrder;
        }

        public void Put(byte[] array, int @from, int copyLength)
        {
            System.Array.Copy(array, @from, _array, _offset + _position, copyLength);
            _position += copyLength;
        }

        public void Get(byte[] ar, int offset, int length)
        {
            if (length > Remaining)
                throw new IndexOutOfRangeException();
            System.Array.Copy(_array, _offset + _position, ar, offset, length);
            _position += length;
        }

        public void Get(byte[] ar)
        {
            Get(ar, 0, ar.Length);
        }

        public void Put(ByteBuffer src, int length)
        {
            Put(src._array, src._offset + src._position, length);
            src._position += length;
        }
    }
}